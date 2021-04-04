using AD.Shared.DTOs.Category;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Helpers
{
    public class LocalizationDB : ILocalizationDB
    {
        public string CurrentCulture = CultureInfo.CurrentCulture.ToString();

        public string GetName(Object Entity)
        {
            string x = "";
            var type = Entity.GetType();
            switch (type.ToString())
            {
                case "AD.Shared.Entities.Country":
                    Country country = Entity as Country;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = country?.Name_ar;
                            break;
                        case "en-US":
                            x = country?.Name_en;
                            break;
                        case "fr-FR":
                            x = country?.Name_fr;
                            break;
                        case "ru-RU":
                            x = country?.Name_ru;
                            break;
                        case "tr-TR":
                            x = country?.Name_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.Entities.City":
                    City city = Entity as City;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = city?.Name_ar;
                            break;
                        case "en-US":
                            x = city?.Name_en;
                            break;
                        case "fr-FR":
                            x = city?.Name_fr;
                            break;
                        case "ru-RU":
                            x = city?.Name_ru;
                            break;
                        case "tr-TR":
                            x = city?.Name_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.Entities.Category":
                    Category category = Entity as Category;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = category?.Name_ar;
                            break;
                        case "en-US":
                            x = category?.Name_en;
                            break;
                        case "fr-FR":
                            x = category?.Name_fr;
                            break;
                        case "ru-RU":
                            x = category?.Name_ru;
                            break;
                        case "tr-TR":
                            x = category?.Name_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.DTOs.Category.CategoryVM":
                    CategoryDTO categoryVM = Entity as CategoryDTO;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = categoryVM?.Category?.Name_ar;
                            break;
                        case "en-US":
                            x = categoryVM?.Category?.Name_en;
                            break;
                        case "fr-FR":
                            x = categoryVM?.Category?.Name_fr;
                            break;
                        case "ru-RU":
                            x = categoryVM?.Category?.Name_ru;
                            break;
                        case "tr-TR":
                            x = categoryVM?.Category?.Name_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.Entities.Currency":
                    Currency currency = Entity as Currency;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = currency?.Code;
                            break;
                        case "en-US":
                            x = currency?.Code;
                            break;
                        case "fr-FR":
                            x = currency?.Code;
                            break;
                        case "ru-RU":
                            x = currency?.Code;
                            break;
                        case "tr-TR":
                            x = currency?.Code;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.Entities.Tag":
                    Tag tag = Entity as Tag;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = tag?.Name_ar;
                            break;
                        case "en-US":
                            x = tag?.Name_en;
                            break;
                        case "fr-FR":
                            x = tag?.Name_fr;
                            break;
                        case "ru-RU":
                            x = tag?.Name_ru;
                            break;
                        case "tr-TR":
                            x = tag?.Name_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.Entities.MeasruingUnit":
                    MeasruingUnit measruingUnit = Entity as MeasruingUnit;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = measruingUnit?.Name_ar;
                            break;
                        case "en-US":
                            x = measruingUnit?.Name_en;
                            break;
                        case "fr-FR":
                            x = measruingUnit?.Name_fr;
                            break;
                        case "ru-RU":
                            x = measruingUnit?.Name_ru;
                            break;
                        case "tr-TR":
                            x = measruingUnit?.Name_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            return x;
        }

        public string GetDescription(Object Entity)
        {
            string x = "";
            var type = Entity.GetType();
            switch (type.ToString())
            {
                case "AD.Shared.Entities.Category":
                    Category category = Entity as Category;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = category?.Description_ar;
                            break;
                        case "en-US":
                            x = category?.Description_en;
                            break;
                        case "fr-FR":
                            x = category?.Description_fr;
                            break;
                        case "ru-RU":
                            x = category?.Description_ru;
                            break;
                        case "tr-TR":
                            x = category?.Description_tr;
                            break;
                        default:
                            break;
                    }
                    break;

                case "AD.Shared.DTOs.Category.CategoryVM":
                    CategoryDTO categoryVM = Entity as CategoryDTO;
                    switch (CurrentCulture)
                    {
                        case "ar-AR":
                            x = categoryVM?.Category?.Description_ar;
                            break;
                        case "en-US":
                            x = categoryVM?.Category?.Description_en;
                            break;
                        case "fr-FR":
                            x = categoryVM?.Category?.Description_fr;
                            break;
                        case "ru-RU":
                            x = categoryVM?.Category?.Description_ru;
                            break;
                        case "tr-TR":
                            x = categoryVM?.Category?.Description_tr;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return x;
        }
    }
}
