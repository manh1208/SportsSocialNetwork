using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class ResponseModel<T>
    {
        public bool Suscess { get; set; }

        public String Message { get; set; }

        public List<String> Errors { get; set; }

        public T Data { get; set; }

        public ResponseModel(bool success, String message, List<String> errorList, T data)
        {
            this.Suscess = success;
            this.Message = message;
            this.Errors = errorList;
            this.Data = data;

        }

        public ResponseModel(bool success, String message, List<String> errorList)
        {
            this.Suscess = success;
            this.Message = message;
            this.Errors = errorList;

        }

        public static ResponseModel<T> CreateErrorResponse(String message, params String[] errors)
        {
            List<String> errorList = new List<string>();

            foreach (var error in errors)
            {
                errorList.Add(error);
            }


            ResponseModel<T> response = new ResponseModel<T>(false, message, errorList);

            return response;
        }

    }
}