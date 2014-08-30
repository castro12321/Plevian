using System.Windows;
using System.Windows.Controls;
namespace Plevian.GUI
{
    public class BetterLabel : Label
    {

        static BetterLabel()
        {

            ContentProperty.OverrideMetadata(typeof(BetterLabel),

                new FrameworkPropertyMetadata(

                    new PropertyChangedCallback(OnContentChanged)));

        }



        private static void OnContentChanged(DependencyObject d,

            DependencyPropertyChangedEventArgs e)
        {

            BetterLabel mcc = d as BetterLabel;

            if (mcc.ContentChanged != null)
            {

                DependencyPropertyChangedEventArgs args

                    = new DependencyPropertyChangedEventArgs(

                        ContentProperty, e.OldValue, e.NewValue);

                mcc.ContentChanged(mcc, args);

            }

        }



        public event DependencyPropertyChangedEventHandler ContentChanged;

    }
}