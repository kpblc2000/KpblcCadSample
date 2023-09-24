using KpblcCadCore.ViewModels.Base;
using KpblcExtensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpblcCadCore.ViewModels
{
    public class BlockAttributeColoringViewModel : ViewModel
    {

        public string AttributeText
        {
            get => _attributeText;
            set
            {
                Set(ref _attributeText, value);
                CanColorAttributeToRed = !string.IsNullOrWhiteSpace(_attributeText);
                CanColorBlockToBlue = !string.IsNullOrWhiteSpace(_attributeText);
                OkButtonAvailable = !string.IsNullOrWhiteSpace(_attributeText);
            }
        }

        /// <summary> Атрибут может быть покрашен в красный </summary>
        public bool CanColorAttributeToRed
        {
            get => _canColorAttributeToRed;
            private set => Set(ref _canColorAttributeToRed, value);
        }

        /// <summary>
        /// Блок может быть покрашен в синий
        /// </summary>
        public bool CanColorBlockToBlue
        {
            get => _canColorBlockToBlue;
            private set => Set(ref _canColorBlockToBlue, value);
        }

        /// <summary>
        /// Красить атрибут в красный
        /// </summary>
        public bool PaintAttributeToRed
        {
            get => _paintAttributeToRed;
            set => Set(ref _paintAttributeToRed, value);
        }

        /// <summary>
        /// Красить блок в синий
        /// </summary>
        public bool PaintBlockToBlue
        {
            get => _paintBlockToBlue;
            set => Set(ref _paintBlockToBlue, value);
        }

        public bool OkButtonAvailable
        {
            get => _OkButtonAvailable;
            private set => Set(ref _OkButtonAvailable, value);
        }

        public WindowResult WinResult
        {
            get => _winRes;
            private set => Set(ref _winRes, value);
        }

        public void OnOkButtonClick()
        {
            WinResult = WindowResult.OK;
        }

        public void OnCancelButtonClick()
        {
            WinResult = WindowResult.Cancel;
        }

        private string _attributeText;
        private bool _canColorAttributeToRed;
        private bool _canColorBlockToBlue;
        private bool _paintAttributeToRed;
        private bool _paintBlockToBlue;
        private bool _OkButtonAvailable;
        public WindowResult _winRes;
    }
}
