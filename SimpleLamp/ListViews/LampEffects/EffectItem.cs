namespace SimpleLamp.ListViews.LampEffects
{
    public class EffectItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public EffectItem(string Name, string Description, int Index) 
        { 
            this.Name = Name;
            this.Description = Description;
            this.Index = Index;
        }

        public EffectItem(string Name, string Description, int Index, string ImageSource)
        {
            this.Name = Name;
            this.Description = Description;
            this.Index = Index;
            this.ImageSource = ImageSource;
        }

        public override string ToString() => this.Name;
    }
}
