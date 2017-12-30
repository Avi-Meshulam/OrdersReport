using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Orders
{
    class EditableComboBox : ComboBox
    {
        private TextBox _editableTextBox;

        public int SelectionStart
        {
            get { return _editableTextBox.SelectionStart; }
            set { _editableTextBox.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return _editableTextBox.SelectionLength; }
            set { _editableTextBox.SelectionLength = value; }
        }

        public override void OnApplyTemplate()
        {
            var myTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            if (myTextBox != null)
            {
                this._editableTextBox = myTextBox;
            }

            base.OnApplyTemplate();
        }

        public void SetCaret(int position)
        {
            _editableTextBox.SelectionStart = position;
            _editableTextBox.SelectionLength = 0;
        }
    }
}
