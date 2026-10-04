using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace testMVC.Models
{
    /// <summary>
    /// نموذج عرض تسجيل الدخول (ViewModel)
    /// يُستخدم فقط لنقل بيانات واجهة تسجيل الدخول (Email & Password)
    /// وفصلها عن نموذج الكينونة الأساسي (Entity Model) لحماية الحقول الأخرى وتعزيز الأمان (Separation of Concerns).
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// البريد الإلكتروني المدخل لتسجيل الدخول
        /// يتم التحقق من إدخاله وصحة نسقه برمجياً قبل إرساله إلى وحدة التحكم
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني لا يمكن أن يكون فارغاً")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صالحة")]
        [Display(Name = "البريد الإلكتروني")]
        public required string Email { get; set; }



        /// <summary>
        /// كلمة المرور المدخلة
        /// نوع الحقل Password يخفي النص أثناء الكتابة
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور لا يمكن أن تكون فارغة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public required string Password { get; set; }
    }
}

