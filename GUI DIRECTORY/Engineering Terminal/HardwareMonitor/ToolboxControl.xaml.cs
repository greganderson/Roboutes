using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HardwareMonitor
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("HardwareMonitor", true)]
    public partial class ToolboxControl : UserControl
    {
        public ToolboxControl()
        {
            InitializeComponent();
        }

        public void setCPUTemp(int temp)
        {
            Action work = delegate
            {
                cpuTempLabel.Text = temp+" °C";
                if (temp >= HWMonitorTools.CPU_MAXSAFETEMP)
                {
                    cpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                }
                else
                {
                    cpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                }
            };
            Dispatcher.Invoke(work);
        }

        public void setGPUTemp(int temp)
        {
            Action work = delegate
            {
                gpuTempLabel.Text = temp + " °C";
                if (temp >= HWMonitorTools.GPU_MAXSAFETEMP)
                {
                    gpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                }
                else
                {
                    gpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                }
            };
            Dispatcher.Invoke(work);
        }

        public void setCPULoad(int load)
        {
            load = load.Constrain(0, 100);
            Dispatcher.Invoke(()=>cpuLoadLabel.Text = load+" %");
        }

        public void setGPULoad(int load)
        {
            load = load.Constrain(0, 100);
            Dispatcher.Invoke(() => gpuLoadLabel.Text = load + " %");
        }

        public void setRAMLoad(int load)
        {
            load = load.Constrain(0, 100);
            Dispatcher.Invoke(() => ramLoadLabel.Text = load + " %");
            if (load >= HWMonitorTools.RAM_MAXSAFEUSAGE)
            {
                Dispatcher.Invoke(() => ramWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Red));
            }
            else
            {
                Dispatcher.Invoke(() => ramWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green));
            }
        }

    }

    public static class HWMonitorTools
    {
        public static readonly int CPU_MAXSAFETEMP = 62;
        public static readonly int GPU_MAXSAFETEMP = 100;

        public static readonly int RAM_MAXSAFEUSAGE = 70;

        public static int Constrain(this int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
    }
}
