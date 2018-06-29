using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

// This is based on David Britch's solution: https://github.com/davidbritch/xamarin-forms/tree/master/ItemSelectedBehavior
// Related blog posts at: 
// - https://blog.xamarin.com/turn-events-into-commands-with-behaviors/
// - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/reusable/event-to-command-behavior
// With a little tidying of code, and comments added to aid my understanding of what's going on

// ReSharper disable once CheckNamespace
namespace MvvmCrossApp
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        private Delegate _eventHandler;

        // Define the 4 bindable properties that can be set by a consumer of this behavior
        // - The name of the existing event (on a View) that will be mapped to a command
        // - The command on the current binding context (likely a view model) that will be hooked up to this event
        // - An optional parameter value specified at the point where the behavior is consumed
        // - An optional converter to map a dynamically defined parameter value to the correct type
        public static readonly BindableProperty EventNameProperty
            = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty
            = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));
        public static readonly BindableProperty CommandParameterProperty
            = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));
        public static readonly BindableProperty InputConverterProperty
            = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(EventToCommandBehavior));

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        // Override the OnAttachedTo and OnDetachingFrom methods to hook up the specified event
        // and un-hook it respectively
        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName, nameof(OnEvent));
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            DeregisterEvent(EventName);
        }

        // Hook up the named event on the associated View to the event handler method on this class
        private void RegisterEvent(string eventName, string eventHandlerName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(eventName);
            if (eventInfo == null)
            {
                throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Can't register the '{eventName}' event.");
            }

            var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod(eventHandlerName);
            _eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, _eventHandler);
        }

        // Un-hook the named event on the associated View
        private void DeregisterEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName) || _eventHandler == null)
            {
                return;
            }

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(eventName);
            if (eventInfo == null)
            {
                throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Can't de-register the '{eventName}' event.");
            }
            eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);
            _eventHandler = null;
        }

        // Once hooked up to the specified event on the associated view, this event handler will obtain
        // the parameter value - which can be provided statically, dynamically or via event args - and 
        // then handle the event by executing the specified ICommand
        private void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
            {
                return;
            }

            object resolvedParameter;

            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
            {
                Command.Execute(resolvedParameter);
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            var oldEventName = (string)oldValue;
            var newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName, nameof(OnEvent));
        }
    }
}