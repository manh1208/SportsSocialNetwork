using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class AjaxOperationResult
    {

        public bool Succeed { get; set; } = true;
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

        public void AddError(string key, string error)
        {
            key = key ?? "";

            List<string> errors;
            if (!this.Errors.TryGetValue(key, out errors))
            {
                errors = new List<string>();
                this.Errors.Add(key, errors);
            }

            errors.Add(error);
        }

        public void AddError<TKey, TElement>(IDictionary<TKey, List<TElement>> errors)
        {
            foreach (var item in errors)
            {
                this.Errors[item.Key.ToString()] = item.Value.Select(q => q.ToString()).ToList();
            }
        }

    }

    public class EntityCreationResult : AjaxOperationResult
    {
        public int CreatedId { get; set; }
    }

    public class EntityCreationResult<T> : EntityCreationResult
    {
        public T CreatedEntity { get; set; }
    }


    public class AjaxOperationResult<T> where T : class
    {

        public bool Succeed { get; set; } = true;
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public T AdditionalData { get; set; }

        public void AddError(string key, string error)
        {
            key = key ?? "";

            List<string> errors;
            if (!this.Errors.TryGetValue(key, out errors))
            {
                errors = new List<string>();
                this.Errors.Add(key, errors);
            }

            errors.Add(error);
        }

        public void AddError<TKey, TElement>(IDictionary<TKey, List<TElement>> errors)
        {
            foreach (var item in errors)
            {
                this.Errors[item.Key.ToString()] = item.Value.Select(q => q.ToString()).ToList();
            }
        }

    }

}