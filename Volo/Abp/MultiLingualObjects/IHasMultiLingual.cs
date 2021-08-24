using System.Collections.Generic;


namespace Volo.Abp.MultiLingualObjects
{
    public interface IHasMultiLingual<TTranslation>
        where TTranslation : class, IMultiLingualTranslation
    {
        string DefaultCulture { get; set; }
        
        ICollection<TTranslation> Translations { get; set; }
    }
}
