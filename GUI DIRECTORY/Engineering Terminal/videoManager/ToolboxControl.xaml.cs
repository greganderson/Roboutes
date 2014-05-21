﻿using System;
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

namespace videoManager
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("videoManager", true)]
    public partial class ToolboxControl : UserControl
    {
        public delegate void IntendedCameraStatusChangedEventHandler(FeedID videoFeedID, bool feedState);
        public event IntendedCameraStatusChangedEventHandler intendedCameraStatusChanged;

        public delegate void ResetRequestEventHandler(FeedID videoFeedID);
        public event ResetRequestEventHandler resetRequest;

        private volatile bool IntendedOculus = false;
        private volatile bool IntendedWorkspace = false;
        private volatile bool IntendedPalm = false;

        public ToolboxControl()
        {
            InitializeComponent();
        }

        public void setReportedStatus(FeedID ID, bool state)
        {
            Action work = delegate
            {
                switch (ID)
                {
                    case FeedID.OculusPT:
                        if (state)
                        {
                            ReportedOculusInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                        }
                        else
                        {
                            ReportedOculusInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                        }
                        break;
                    case FeedID.Workspace:
                        if (state)
                        {
                            ReportedWorkspaceInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                        }
                        else
                        {
                            ReportedWorkspaceInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                        }
                        break;
                    case FeedID.Palm:
                        if (state)
                        {
                            ReportedPalmInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                        }
                        else
                        {
                            ReportedPalmInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                        }
                        break;
                }
            };

            Dispatcher.Invoke(work);
        }

        public enum FeedID
        {
            OculusPT,
            Workspace,
            Palm
        }

        private void OculusToggle_Click(object sender, RoutedEventArgs e)
        {
            IntendedOculus = !IntendedOculus;
            if (IntendedOculus)
            {
                Dispatcher.Invoke(()=>IntendedOculusInd.setIndicatorState(toggleIndicator.indicatorState.Green));
                if (intendedCameraStatusChanged != null)
                {
                    intendedCameraStatusChanged(FeedID.OculusPT, true);
                }
            }
            else
            {
                IntendedOculusInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                if (intendedCameraStatusChanged != null)
                {
                    intendedCameraStatusChanged(FeedID.OculusPT, false);
                }
            }
        }

        private void WorkspaceToggle_Click(object sender, RoutedEventArgs e)
        {
            IntendedWorkspace = !IntendedWorkspace;
            if (IntendedWorkspace)
            {
                Dispatcher.Invoke(()=>IntendedWorkspaceInd.setIndicatorState(toggleIndicator.indicatorState.Green));
                if (intendedCameraStatusChanged != null)
                {
                    intendedCameraStatusChanged(FeedID.Workspace, true);
                }
            }
            else
            {
                IntendedWorkspaceInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                if (intendedCameraStatusChanged != null)
                {
                    intendedCameraStatusChanged(FeedID.Workspace, false);
                }
            }
        }

        private void PalmToggle_Click(object sender, RoutedEventArgs e)
        {
            IntendedPalm = !IntendedPalm;
            if (IntendedPalm)
            {
                Dispatcher.Invoke(()=>IntendedPalmInd.setIndicatorState(toggleIndicator.indicatorState.Green));
                if (intendedCameraStatusChanged != null)
                {
                    intendedCameraStatusChanged(FeedID.Palm, true);
                }
            }
            else
            {
                IntendedPalmInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                if (intendedCameraStatusChanged != null)
                {
                    intendedCameraStatusChanged(FeedID.Palm, false);
                }
            }
        }

        private void OculusReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MR = MessageBox.Show("Are you sure you want to attempt to reset the Oculus video feed?");
            if (MR == MessageBoxResult.Yes)
            {
                if (resetRequest != null)
                {
                    resetRequest(FeedID.OculusPT);
                }
            }
        }

        private void WorkspaceReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MR = MessageBox.Show("Are you sure you want to attempt to reset the Workspace video feed?");
            if (MR == MessageBoxResult.Yes)
            {
                if (resetRequest != null)
                {
                    resetRequest(FeedID.Workspace);
                }
            }
        }

        private void PalmReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MR = MessageBox.Show("Are you sure you want to attempt to reset the Palm video feed?");
            if (MR == MessageBoxResult.Yes)
            {
                if (resetRequest != null)
                {
                    resetRequest(FeedID.Palm);
                }
            }
        }

    }
}
