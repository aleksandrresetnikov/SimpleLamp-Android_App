using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleLamp.ListViews.LampEffects
{
    public class ListViewEffectsGroup : List<EffectItem>
    {
        public bool ContainsName(string NameValue)
        {
            foreach (EffectItem item in this)
                if (item.Name == NameValue && item.Name.Length == NameValue.Length)
                    return true;

            return false;
        }

        public bool ContainsDescription(string DescriptionValue)
        {
            foreach (EffectItem item in this)
                if (item.Description == DescriptionValue)
                    return true;

            return false;
        }

        public bool ContainsIndex(int Index)
        {
            foreach (EffectItem item in this)
                if (item.Index == Index)
                    return true;

            return false;
        }

        public bool ContainsImageSource(string ImageSourceValue)
        {
            foreach (EffectItem item in this)
                if (item.ImageSource == ImageSourceValue)
                    return true;

            return false;
        }

        public EffectItem GetEffectItem(int Index)
        {
            foreach (EffectItem item in this)
                if (item.Index == Index)
                    return item;

            return null;
        }

        public EffectItem GetEffectItem(string Name)
        {
            foreach (EffectItem item in this)
                if (item.Name == Name)
                    return item;

            return null;
        }

        public int GetEffectItemIndex(string Name)
        {
            foreach (EffectItem item in this)
                if (item.Name == Name)
                    return item.Index;

            return -1;
        }
    }
}
