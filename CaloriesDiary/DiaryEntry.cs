//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaloriesDiary
{
    using System;
    using System.Collections.Generic;
    
    public partial class DiaryEntry
    {
        public int id { get; set; }
        public Nullable<int> quantity { get; set; }
        public int foodid { get; set; }
        public int diaryid { get; set; }
        public int measureid { get; set; }
    
        public virtual Diary Diary { get; set; }
        public virtual Measure Measure { get; set; }
        public virtual Food Food { get; set; }
    }
}
