using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro.Autofac;
using Timebox.ViewModels;
using System.Windows;
using System.Windows.Media;
using Autofac.Core;
using Timebox.Views;

namespace Timebox.Models
{
    public class Bootstrapper: AutofacBootstrapper<ShellViewModel>
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<ImageSticker>().As<ImageSticker>().SingleInstance();
            builder.RegisterType<Attendees>().As<Attendees>();
            builder.RegisterType<ChuckApi>().As<ChuckApi>();
            builder.RegisterType<QuotesViewModel>().As<QuotesViewModel>();
            builder.RegisterType<ClockViewModel>().As<ClockViewModel>();
            builder.RegisterType<AppConfig>().As<AppConfig>().SingleInstance();
            builder.RegisterType<TunePlayer>().As<TunePlayer>().SingleInstance();
            builder.RegisterType<ControllerViewModel>().As<ControllerViewModel>();
            builder.RegisterType<ShellViewModel>();
        }
    }
}
