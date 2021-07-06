using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Bed { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CategoryModel)
            {
                var objCM = obj as CategoryModel;
                return this.Id == objCM.Id
                    && this.Name == objCM.Name
                    && this.Bed == objCM.Bed;
            }
            else
            {
                return base.Equals(obj);
            }
        }

    }
}