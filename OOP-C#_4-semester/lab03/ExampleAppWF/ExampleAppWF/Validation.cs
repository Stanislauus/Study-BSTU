using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAppWF
{
    public static class Validation
    {
        public static List<ValidationResult> Validate(object obj)
        {
            var validationResults = new List<ValidationResult>();   
            var context = new ValidationContext(obj, null, null); // 2.провайдер служб, 3.словарь для доп. служб
            Validator.TryValidateObject(obj, context, validationResults, true);

            foreach (var property in obj.GetType().GetProperties())
            {
                // Если свойство - класс (но не строка!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    var value = property.GetValue(obj);
                    if (value != null)
                    {
                        // Проверка на коллекцию
                        if (value is IEnumerable collection && !(value is string))
                        {
                            foreach (var item in collection)
                            {
                                if (item == null) continue;
                                var itemResults = Validate(item);
                                validationResults.AddRange(itemResults);
                            }
                        }
                        else // Одиночный объект
                        {
                            var nestedResults = Validate(value);
                            validationResults.AddRange(nestedResults);
                        }
                    }
                }
            }
            return validationResults;
        }
    }
}
