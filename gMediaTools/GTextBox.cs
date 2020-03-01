using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gMediaTools
{
    public class GTextBox : TextBox
    {
        public enum GTextBoxType
        {
            Text,
            Numeric,
            Data
        }

        protected GTextBoxType _TextBoxType = GTextBoxType.Text;

        [Browsable(true)]
        public GTextBoxType TextBoxType
        {
            get { return _TextBoxType; }
            set
            {
                _TextBoxType = value;
                switch (_TextBoxType)
                {
                    case GTextBoxType.Text:
                        break;
                    case GTextBoxType.Numeric:
                        TextAlign = HorizontalAlignment.Right;
                        break;
                    case GTextBoxType.Data:
                        break;
                    default:
                        break;
                }
                OnTextChanged(null);
            }
        }

        protected Int32 _Decimals = 2;

        [Browsable(true)]
        public Int32 Decimals
        {
            get { return _Decimals; }
            set
            {
                _Decimals = value;
                OnTextChanged(null);
            }
        }

        protected Object _DataObject = null;

        [Browsable(true)]
        public Object DataObject
        {
            get { return _DataObject; }
            set
            {
                _DataObject = value;
                if (_DataObject == null)
                {
                    this.Text = String.Empty;
                }
                else
                {
                    this.Text = _DataObject.ToString();
                }
            }
        }

        [Browsable(false)]
        public Decimal DecimalValue
        {
            get
            {
                if (_TextBoxType != GTextBoxType.Numeric)
                {
                    return 0m;
                }
                if (String.IsNullOrWhiteSpace(Text))
                {
                    return 0m;
                }
                return Decimal.Parse(Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                if (_TextBoxType != GTextBoxType.Numeric)
                {
                    return;
                }
                // If value decimals are more than the property, Round the value
                Int32 decimals = value.GetDecimals();
                if (decimals > Decimals)
                {
                    value = Decimal.Round(value, Decimals, MidpointRounding.AwayFromZero);
                }
                Text = value.ToString();
            }
        }

        [Browsable(false)]
        public Int32 Int32Value
        {
            get
            {
                if (_TextBoxType != GTextBoxType.Numeric)
                {
                    return 0;
                }
                if (String.IsNullOrWhiteSpace(Text))
                {
                    return 0;
                }
                return Int32.Parse(Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                if (_TextBoxType != GTextBoxType.Numeric)
                {
                    return;
                }
                Text = value.ToString();
            }
        }

        [Browsable(false)]
        public Int64 Int64Value
        {
            get
            {
                if (_TextBoxType != GTextBoxType.Numeric)
                {
                    return 0;
                }
                if (String.IsNullOrWhiteSpace(Text))
                {
                    return 0;
                }
                return Int64.Parse(Text, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                if (_TextBoxType != GTextBoxType.Numeric)
                {
                    return;
                }
                Text = value.ToString();
            }
        }

        public GTextBox()
            : base()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GTextBox
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ResumeLayout(false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            try
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    this.SelectAll();
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    Clipboard.SetText(this.SelectedText, TextDataFormat.UnicodeText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            switch (_TextBoxType)
            {
                case GTextBoxType.Text:
                    break;
                case GTextBoxType.Numeric:
                    // Remove everything except from numbers, ".", "," and "-"
                    if (((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != '-')))
                    {
                        e.Handled = true;
                    }
                    // Allow only one minus character "-"
                    if ((e.KeyChar == '-' && Text.Contains("-")))
                    {
                        e.Handled = true;
                    }
                    // Allow only one decimal separator
                    // Check for "."
                    if ((e.KeyChar == '.' && (Text.Contains(".") || Text.Contains(','))))
                    {
                        e.Handled = true;
                    }
                    // Check for ","
                    if ((e.KeyChar == ',' && (Text.Contains(".") || Text.Contains(','))))
                    {
                        e.Handled = true;
                    }
                    break;
                case GTextBoxType.Data:
                    this.ReadOnly = true;
                    if (_DataObject == null)
                    {
                        this.Text = String.Empty;
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            switch (_TextBoxType)
            {
                case GTextBoxType.Text:
                    break;
                case GTextBoxType.Numeric:
                    // Save the position and selection of the caret
                    Int32 start = SelectionStart;
                    Int32 length = SelectionLength;

                    // Prepare the text for decimal conversion with InvariantCulture
                    this.Text = this.Text.PrepareStringForNumericParse();
                    // Check if the number can be parsed to Decimal
                    decimal tmpDecimal = default(decimal);
                    if ((!decimal.TryParse(Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out tmpDecimal)))
                    {
                        Text = string.Empty;
                    }
                    else
                    {
                        // Check if it has decimals, in order to keep 4 or the according property
                        if ((Text.Contains(".")))
                        {
                            if (_Decimals > 0)
                            {
                                if ((Text.Split(new string[] { "." }, StringSplitOptions.None)[1].Length > _Decimals))
                                {
                                    // Truncate the decimals
                                    Text = string.Format("{0}.{1}",
                                        Text.Split(new string[] { "." }, StringSplitOptions.None)[0],
                                        Text.Split(new string[] { "." }, StringSplitOptions.None)[1].Substring(0, _Decimals));
                                }
                            }
                            else
                            {
                                Text = Text.Split(new string[] { "." }, StringSplitOptions.None)[0];
                            }
                        }
                    }

                    // Restore the position and selection of the caret
                    Select(start, length);
                    break;
                case GTextBoxType.Data:
                    this.ReadOnly = true;
                    if (String.IsNullOrWhiteSpace(this.Text))
                    {
                        _DataObject = null;
                    }
                    if (_DataObject == null)
                    {
                        this.Text = String.Empty;
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnReadOnlyChanged(EventArgs e)
        {
            base.OnReadOnlyChanged(e);

            switch (_TextBoxType)
            {
                case GTextBoxType.Text:
                    break;
                case GTextBoxType.Numeric:
                    break;
                case GTextBoxType.Data:
                    if (!this.ReadOnly)
                    {
                        this.ReadOnly = true;
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            base.OnTextAlignChanged(e);

            switch (_TextBoxType)
            {
                case GTextBoxType.Text:
                    break;
                case GTextBoxType.Numeric:
                    TextAlign = HorizontalAlignment.Right;
                    break;
                case GTextBoxType.Data:
                    break;
                default:
                    break;
            }
        }
    }
}
