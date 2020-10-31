using System;
using System.Collections.Generic;
using System.Text;

namespace Tgs.DataContract
{
    public class CustomSelectListItem
    {
        public CustomSelectListItem() { }
        public CustomSelectListItem(string text, string value, string icon) { }
        public CustomSelectListItem(string text, string value, string icon, bool selected) { }
        public CustomSelectListItem(string text, string value, string icon, bool selected, bool disabled) { }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string[] Icon { get; set; }
    }
}
