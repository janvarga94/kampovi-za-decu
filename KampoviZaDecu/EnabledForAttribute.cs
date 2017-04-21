using System;

namespace KampoviZaDecu
{
    internal class EnabledForAttribute : Attribute
    {
        private string bindingProperty;
        public string BindingProperty
        {
            get
            {
                return bindingProperty;
            }
            set
            {
                bindingProperty = value;
            }
        }

        public EnabledForAttribute(string v)
        {
            this.bindingProperty = v;
        }
    }
}