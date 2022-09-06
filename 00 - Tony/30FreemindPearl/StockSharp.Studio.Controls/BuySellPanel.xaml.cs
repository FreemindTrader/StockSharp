﻿using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Ecng.Serialization;
using Ecng.Xaml;
using StockSharp.Algo;
using StockSharp.BusinessEntities;
using StockSharp.Messages;
using StockSharp.Studio.Core;
using StockSharp.Studio.Core.Commands;
using StockSharp.Xaml;
using StockSharp.Xaml.PropertyGrid;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace StockSharp.Studio.Controls
{
    public partial class BuySellPanel : BaseStudioControl, IComponentConnector
    {
        private readonly HashSet<Window> _windows = new HashSet<Window>();
        

        public BuySellSettings Settings { get; } = new BuySellSettings();

        public BuySellPanel()
        {
            this.InitializeComponent();
            this.Settings.PropertyChanged += ( PropertyChangedEventHandler )( ( _1, _2 ) => this.RaiseChangedCommand() );
            this.SettingsPropertyGrid.SelectedObject = ( object )this.Settings;
            this.Loaded += ( RoutedEventHandler )( ( _1, _2 ) =>
              {
                  this.TrySubscribeWindow( Application.Current.MainWindow );
                  this.TrySubscribeWindow( Window.GetWindow( ( DependencyObject )this ) );
              } );
        }

        private void TrySubscribeWindow( Window win )
        {
            if ( win == null || this._windows.Contains( win ) )
                return;
            this._windows.Add( win );
            win.PreviewMouseDown += ( MouseButtonEventHandler )( ( _, args ) =>
              {
                  if ( !this.SettingsPopup.IsOpen || this.IsInsidePopup( args.OriginalSource ) )
                      return;
                  this.SettingsPopup.IsOpen = false;
              } );
        }

        private bool IsInsidePopup( object src )
        {
            for ( DependencyObject reference = src as DependencyObject; reference != null; reference = VisualTreeHelper.GetParent( reference ) )
            {
                if ( reference == this.SettingsPanel )
                    return true;
            }
            return false;
        }

        public IStudioControl Host { get; set; }

        private void SendRegisterOrder( Sides direction, Decimal price = 0M, OrderTypes type = OrderTypes.Market )
        {
            if ( this.Host == null )
                return;
            if ( this.Settings.Security == null )
            {
                SecurityPickerWindow wnd = new SecurityPickerWindow() { SecurityProvider = ServicesRegistry.SecurityProvider, SelectionMode = MultiSelectMode.Row };
                if ( wnd.ShowModal( ( DependencyObject )this ) )
                    this.Settings.Security = wnd.SelectedSecurity;
                if ( this.Settings.Security == null )
                    return;
            }
            if ( this.Settings.Portfolio == null )
            {
                PortfolioPickerWindow wnd = new PortfolioPickerWindow() { Portfolios = StudioServicesRegistry.PortfolioDataSource };
                if ( wnd.ShowModal( ( DependencyObject )this ) )
                    this.Settings.Portfolio = wnd.SelectedPortfolio;
                if ( this.Settings.Portfolio == null )
                    return;
            }
            this.Host.SendCommand( ( IStudioCommand )new RegisterOrderCommand( new Order()
            {
                Portfolio = this.Settings.Portfolio,
                Security = this.Settings.Security,
                Direction = direction,
                Price = price,
                Volume = this.Settings.Volume,
                Type = new OrderTypes?( type )
            } ) );
        }

        private void BuyAtLimit_Click( object sender, RoutedEventArgs e )
        {
            if ( this.Settings.LimitPrice <= Decimal.Zero )
                return;
            this.SendRegisterOrder( Sides.Buy, this.Settings.LimitPrice, OrderTypes.Limit );
        }

        private void SellAtLimit_Click( object sender, RoutedEventArgs e )
        {
            if ( this.Settings.LimitPrice <= Decimal.Zero )
                return;
            this.SendRegisterOrder( Sides.Sell, this.Settings.LimitPrice, OrderTypes.Limit );
        }

        private void BuyAtMarket_Click( object sender, RoutedEventArgs e )
        {
            this.SendRegisterOrder( Sides.Buy, Decimal.Zero, OrderTypes.Market );
        }

        private void SellAtMarket_Click( object sender, RoutedEventArgs e )
        {
            this.SendRegisterOrder( Sides.Sell, Decimal.Zero, OrderTypes.Market );
        }

        private void ClosePosition_Click( object sender, RoutedEventArgs e )
        {
            if ( this.Host == null || this.Settings.Security == null )
                return;
            this.Host.SendCommand( ( IStudioCommand )new ClosePositionCommand( this.Settings.Security ) );
        }

        private void RevertPosition_Click( object sender, RoutedEventArgs e )
        {
            if ( this.Host == null || this.Settings.Security == null )
                return;
            this.Host.SendCommand( ( IStudioCommand )new RevertPositionCommand( this.Settings.Security ) );
        }

        private void CancelAll_Click( object sender, RoutedEventArgs e )
        {
            if ( this.Host == null )
                return;
            this.Host.SendCommand( ( IStudioCommand )new CancelAllOrdersCommand() );
        }

        public override void Save( SettingsStorage storage )
        {
            storage.SetValue<SettingsStorage>( "Settings", this.Settings.Save() );
            base.Save( storage );
        }

        public override void Load( SettingsStorage storage )
        {
            SettingsStorage storage1 = storage.GetValue<SettingsStorage>( "Settings", ( SettingsStorage )null );
            if ( storage1 != null )
                this.Settings.Load( storage1 );
            base.Load( storage );
        }

        
    }
}
