﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SpacePewPew.GameLogic;
using SpacePewPew.UI.Proxy.UICreators;

namespace SpacePewPew.UI.Proxy
{
    public class LayoutManager
    {
        #region Singleton

        private LayoutManager(ScreenType screenType)
        {
            ScreenType = screenType;
            IsShowingModal = false;
            Components = new List<UiElement>();
            ModalComponents = new List<UiElement>();
            SetComponents(screenType);
        }

        public static LayoutManager GetManager()
        {
            return _ins ?? (_ins = new LayoutManager(ScreenType.MainMenu));
        }

        private static LayoutManager _ins;

        #endregion

        private bool _showingModal;

        public bool IsShowingModal
        {
            get
            {
                return _showingModal;
            }
            set
            {
                Game.Instance().TimerEnabled = !value;
                _showingModal = value;
            }
        }

        public List<UiElement> Components { get; set; }
        public List<UiElement> ModalComponents { get; set; } 
        public ScreenType ScreenType { get; set; }

        private IUiCreator _builder;
        
        public void SetComponents(ScreenType screenType) //установка набора компонентов для нынешнего состояния игры
        {
            switch (screenType)
            {
                case ScreenType.MainMenu:
                    {
                        _builder = new MainMenuUICreator();
                        break;
                    }
                case ScreenType.Game:
                    {
                        _builder = new GameUICreator();
                        break;
                    }
                case ScreenType.Editor:
                    _builder = new EditorUICreator();
                    break;
            }
           
            _builder.Create(Components, ModalComponents);
        }


        public bool OnClick(PointF pos)
        {
            return IsShowingModal ? ModalComponents.Select(item => item.Click(pos)).Any(clickProcessed => clickProcessed) 
                : Components.Select(item => item.Click(pos)).Any(clickProcessed => clickProcessed);
        }

        public UiElement GetComponent(Type type, bool isModal) //TODO: придумать
        {
            return isModal ? ModalComponents.FirstOrDefault(item => item.GetType() == type) :
                                  Components.FirstOrDefault(item => item.GetType() == type);
        }
    }
}
