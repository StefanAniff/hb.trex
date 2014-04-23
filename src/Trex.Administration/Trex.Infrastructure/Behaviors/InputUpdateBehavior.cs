using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Trex.Infrastructure.Behaviors
{
    public class InputUpdateBehavior:Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.KeyUp += new System.Windows.Input.KeyEventHandler(AssociatedObject_KeyUp);
            
        }

        void AssociatedObject_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var binding = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            if(binding != null)
                binding.UpdateSource();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.KeyUp -= AssociatedObject_KeyUp;

        }
    }
}
