using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Windows_CLient.UserControls
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : UserControl
    {
        public Task()
        {
            InitializeComponent();
        }

        public Visibility visibility
        {
            get { return (Visibility)GetValue(visibilityProperty); }
            set { SetValue(visibilityProperty, value); }
        }

        public static readonly DependencyProperty visibilityProperty =
            DependencyProperty.Register("visibility", typeof(Visibility), typeof(Task), new PropertyMetadata(Visibility.Hidden));

        //TODO : Complete the collapse / show feature
    }
}
