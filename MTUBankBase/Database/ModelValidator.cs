using EmbedIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Database
{
    public class ModelValidator
    {
        /// <summary>
        /// Проверяет наличие всех обязательных полей в модели. При отсутствии хотя бы одного поля выбрасывает исключение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool ValidateModel(object model)
        {
            // get all properties of a model
            var type = model.GetType();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var classAttr = type.GetCustomAttribute(typeof(TableAttribute));

            // check that all properties that (have [Required] attribute) OR (don't have GetOnlyJsonProperty attr AND are a DB model) are non-null
            foreach (var prop in props)
            {
                bool flag = false;

                // get value first
                var val = prop.GetValue(model);
                if (val != null) continue;

                // check for req attr
                var attr = prop.GetCustomAttribute(typeof(RequiredAttribute));
                if (attr is not null) flag = true;

                // check if base class has the attribute
                if (classAttr is null && prop.GetCustomAttribute(typeof(GetOnlyJsonPropertyAttribute)) is null) flag = true;

                if (flag) throw new HttpException(System.Net.HttpStatusCode.BadRequest, $"Property [{prop.Name}] is null, a value was expected.");
            }

            return true;
        }
    }
}
