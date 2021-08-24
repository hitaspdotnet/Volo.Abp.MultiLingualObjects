using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp.DependencyInjection;


namespace Volo.Abp.MultiLingualObjects
{
    public class MultiLingualObjectManager : IMultiLingualObjectManager, ITransientDependency
    {
        protected const int MaxCultureFallbackDepth = 5;

        public MultiLingualObjectManager()
        {
        }

        public virtual TTranslation GetTranslation<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            string culture = null,
            bool fallbackToParentCultures = true)
            where TMultiLingual : IHasMultiLingual<TTranslation>
            where TTranslation : class, IMultiLingualTranslation
        {
            culture ??= CultureInfo.CurrentUICulture.Name;

            if (multiLingual.Translations.IsNullOrEmpty())
            {
                return null;
            }

            var translation = multiLingual.Translations.FirstOrDefault(pt => pt.Culture == culture);
            if (translation != null)
            {
                return translation;
            }

            if (fallbackToParentCultures)
            {
                translation = GetTranslationBasedOnCulturalRecursive(
                    CultureInfo.CurrentUICulture.Parent,
                    multiLingual.Translations,
                    0
                );
                
                if (translation != null)
                {
                    return translation;
                }
            }

            translation = multiLingual.Translations.FirstOrDefault(pt => pt.Culture == multiLingual.DefaultCulture);
            if (translation != null)
            {
                return translation;
            }

            translation = multiLingual.Translations.FirstOrDefault();
            return translation;
        }

        protected virtual TTranslation GetTranslationBasedOnCulturalRecursive<TTranslation>(
            CultureInfo culture, ICollection<TTranslation> translations, int currentDepth)
            where TTranslation : class, IMultiLingualTranslation
        {
            if (culture == null ||
                culture.Name.IsNullOrWhiteSpace() ||
                translations.IsNullOrEmpty() ||
                currentDepth > MaxCultureFallbackDepth)
            {
                return null;
            }

            var translation = translations.FirstOrDefault(pt => pt.Culture.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
            return translation ?? GetTranslationBasedOnCulturalRecursive(culture.Parent, translations, currentDepth + 1);
        }
    }
}
