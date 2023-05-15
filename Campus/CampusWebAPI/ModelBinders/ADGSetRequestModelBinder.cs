using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Campus.Core.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;

public class ADGSetRequestModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        try
        {
            // Отримуємо тіло запиту
            var body = bindingContext.HttpContext.Request.Body;

            // Зчитуємо тіло запиту як рядок
            using (var reader = new StreamReader(body))
            {
                var json = await reader.ReadToEndAsync();

                // Розпарсюємо JSON у JObject
                var jobject = JObject.Parse(json);

                // Створюємо екземпляр ADGSetRequest
                var request = new ADGSetRequest();

                // Заповнюємо властивості AcademicId та DisciplineGroupsRelation
                if (jobject.TryGetValue("academicId", out var academicIdToken))
                {
                    request.AcademicId = Guid.Parse(academicIdToken.ToString());
                }

                if (jobject.TryGetValue("disciplineGroupsRelation", out var dgrToken) && dgrToken is JArray dgrArray)
                {
                    request.DisciplineGroupsRelation = new Dictionary<Guid, IEnumerable<Guid>>();

                    foreach (var item in dgrArray)
                    {
                        if (item is JObject itemObject && itemObject.TryGetValue("disciplineId", out var disciplineIdToken) && itemObject.TryGetValue("groups", out var groupsToken) && groupsToken is JArray groupsArray)
                        {
                            var disciplineId = Guid.Parse(disciplineIdToken.ToString());
                            var groupIds = groupsArray.Select(token => Guid.Parse(token.ToString())).ToList();
                            request.DisciplineGroupsRelation.Add(disciplineId, groupIds);
                        }
                    }
                }

                // Додаємо модель до контексту
                bindingContext.Result = ModelBindingResult.Success(request);
            }
        }
        catch (Exception)
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
