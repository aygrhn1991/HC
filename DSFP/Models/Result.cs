using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSFP.Models
{
    public static class Result
    {
        public static ResultModel AutoResult(int data)
        {
            ResultModel model = new ResultModel();
            model.success = data == 1 ? true : false;
            model.message = data == 1 ? "操作成功" : "操作失败";
            model.count = 0;
            model.data = data;
            return model;
        }
        public static ResultModel Success(int count, object data)
        {
            ResultModel model = new ResultModel();
            model.success = true;
            model.message = null;
            model.count = count;
            model.data = data;
            return model;
        }
        public static ResultModel Error(string message, object data)
        {
            ResultModel model = new ResultModel();
            model.success = false;
            model.message = message;
            model.count = 0;
            model.data = data;
            return model;
        }
    }

    public class ResultModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int count { get; set; }
        public object data { get; set; }
    }
}