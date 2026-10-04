using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace testMVC.Models
{
    /// <summary>
    /// نموذج كينونة المستخدم (User Entity Model)
    /// يمثل جدول المستخدمين في قاعدة البيانات عبر تقنية Entity Framework Core (Code-First)
    /// </summary>
    
    // [Index]: إنشاء فهرس فريد على مستوى قاعدة البيانات لمنع تكرار البريد الإلكتروني برمجياً على مستوى الـ Database
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        /// <summary>
        /// المعرف الأساسي للمستخدم (Primary Key)
        /// يتم ترقيمه تلقائياً (Identity / Auto-Increment) في SQL Server
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// اسم المستخدم (User Name)
        /// [Required]: التحقق من أن الحقل إلزامي ولا يمكن تركه فارغاً
        /// [Remote]: فحص لحظي غير متزامن عبر Ajax للتأكد من عدم تكرار الاسم في قاعدة البيانات قبل إرسال النموذج
        /// </summary>
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Remote(action: "IsUsernameAvailable", controller: "User", ErrorMessage = "هذا اسم المستخدم مستخدم بالفعل")]
        [Display(Name = "اسم المستخدم")]
        public required string userName { get; set; }

        /// <summary>
        /// البريد الإلكتروني (Email Address)
        /// [EmailAddress]: فحص صحة وتنسيق الإيميل (وجود @ واسم النطاق)
        /// [Remote]: فحص فوري على الخادم للتحقق من عدم تكرار البريد مسبقاً
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [Remote(action: "IsEmailAvailable", controller: "User", ErrorMessage = "هذا البريد الإلكتروني مستخدم بالفعل")]
        [Display(Name = "البريد الإلكتروني")]
        public required string Email { get; set; }

        /// <summary>
        /// كلمة المرور (Password)
        /// [DataType(DataType.Password)]: إخفاء الأحرف بنقاط سوداء في واجهة المستخدم تلقائياً
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public required string Password { get; set; }
    }
}

