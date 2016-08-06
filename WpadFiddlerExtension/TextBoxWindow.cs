using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WpadFiddlerExtension
{

class TextBoxWindow : Form {
   private TextBox mTextBox;
   private Button  mOK;
   private Button  mCancel;
   private string  mContent;
   private static int sBuffer = 4;

   public string Content { get { return mContent; } }

   private void EventOKClicked(object sender, EventArgs args) {
      mContent = mTextBox.Text;
      DialogResult = DialogResult.OK;
      Dispose();
   }

   private void EventCancelClicked(object sender, EventArgs args) {
      mContent = mTextBox.Text;
      DialogResult = DialogResult.Cancel;
      Dispose();
   }

   public TextBoxWindow(string title, string prompt, string content, 
    bool displayCancel, bool singleLine) : base() {
      Font font = new Font(
       new FontFamily("Lucida Console"),
       12,
       FontStyle.Regular,
       GraphicsUnit.Pixel);

      SuspendLayout();

      Size size = ClientSize;
      size.Width = (int)(size.Width * 1.5);
      ClientSize = size;

      DialogResult = DialogResult.OK;
      Label label = new Label();
      label.Text = prompt;
      label.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
      label.Top = ClientRectangle.Top + sBuffer;
      label.Left = ClientRectangle.Left + sBuffer;
      label.Width = ClientRectangle.Width - 2 * sBuffer;
      label.Height = 20;
      Controls.Add(label);

      mTextBox = new TextBox();
      mTextBox.Multiline = !singleLine;
      mTextBox.ScrollBars = ScrollBars.Both;
      mTextBox.Anchor = 
       AnchorStyles.Top | 
       AnchorStyles.Right | 
       AnchorStyles.Left | 
       AnchorStyles.Bottom;
      mTextBox.Top = label.Top + label.Height + sBuffer;
      mTextBox.Height = singleLine ? 20 : 300;
      mTextBox.Left = sBuffer;
      mTextBox.Width = ClientRectangle.Width - 2 * sBuffer;
      mTextBox.Font = font;
      mTextBox.ReadOnly = !displayCancel;
      mTextBox.Text = content;
      Controls.Add(mTextBox);

      mOK = new Button();
      AcceptButton = mOK;
      mOK.Text = "OK";
      mOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      mOK.Top = mTextBox.Top + mTextBox.Height + sBuffer;
      mOK.Left = ClientRectangle.Width - (mOK.Width + sBuffer);
      mOK.Click += new EventHandler(EventOKClicked);
      Controls.Add(mOK);

      mCancel = new Button();
      CancelButton = mCancel;
      mCancel.Text = "Cancel";
      mCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      mCancel.Top = mTextBox.Top + mTextBox.Height + sBuffer;
      mCancel.Left = ClientRectangle.Width - (mCancel.Width + sBuffer);
      mCancel.Click += new EventHandler(EventCancelClicked);

      if (displayCancel) {
         mOK.Left = mCancel.Left - (mCancel.Width + sBuffer);
         Controls.Add(mCancel);
      }

      size = ClientSize;
      size.Height = (mOK.Bottom + sBuffer);
      ClientSize = size;

      Text = title;
      StartPosition = FormStartPosition.CenterScreen;
      ControlBox = true;
      MaximizeBox = false;
      MinimizeBox = false;
      FormBorderStyle = FormBorderStyle.SizableToolWindow;
      MinimumSize = Size;
      mOK.Focus();

      ResumeLayout(false);
      PerformLayout();

      ShowDialog();
   }
}

}
