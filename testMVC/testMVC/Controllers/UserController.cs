using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testMVC.Data;
using testMVC.Models;

namespace testMVC.Controllers
{
    /// <summary>
    /// متحكم إدارة المستخدمين (UserController)
    /// مسؤول عن استقبال طلبات المتصفح، معالجة منطق العمل (Business Logic)، 
    /// التخاطب مع قاعدة البيانات عبر Entity Framework Core، وتوجيه البيانات إلى صفحات العرض (Views).
    /// </summary>
    public class UserController : Controller
    {
        // سياق قاعدة البيانات (DbContext) للتعامل مع الجداول
        private readonly AppDbContext _context;

        /// <summary>
        /// حقن الاعتمادية (Dependency Injection - DI)
        /// يتم تمرير كائن AppDbContext تلقائياً من نظام ASP.NET Core دون الحاجة لإنشائه يدوياً بـ new
        /// </summary>
        public UserController(AppDbContext appCon)
        {
            _context = appCon;
        }

        // ==========================================
        // 1. صفحة استعراض المستخدمين (Read / Index)
        // ==========================================
        /// <summary>
        /// استرجاع قائمة جميع المستخدمين من قاعدة البيانات وعرضها
        /// استخدام async / await يمنع حظر خيط المعالجة (Non-blocking I/O) مما يحسن أداء وسرعة النظام
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var u = await _context.Users.ToListAsync();
            return View(u); // تمرير القائمة إلى View ليتم عرضها كبطاقات أو جدول ملكي
        }

        // =========================================================
        // 2. التحقق اللحظي غير المتزامن (Remote Validation / Ajax)
        // =========================================================
        /// <summary>
        /// فحص فوري ومباشر لتوفر البريد الإلكتروني قبل إرسال النموذج
        /// [AcceptVerbs]: يسمح باستقبال الطلب سواء بطريقة GET أو POST
        /// المعامل id: في حال التعديل (Edit) يتم استثناء المستخدم الحالي من الفحص
        /// </summary>
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsEmailAvailable(string email, int id = 0)
        {
            // إذا كان البريد موجوداً لمستخدم آخر غير الحالي
            bool exists = _context.Users.Any(u => u.Email == email && u.Id != id);

            if (exists)
            {
                return Json($"البريد الإلكتروني '{email}' مُستخدم بالفعل.");
            }

            return Json(true); // البريد متاح وصالح للاستخدام
        }

        /// <summary>
        /// فحص فوري ومباشر لتوفر اسم المستخدم لمنع تكرار الأسماء
        /// </summary>
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsUsernameAvailable(string userName, int id = 0)
        {
            bool exists = _context.Users.Any(u => u.userName == userName && u.Id != id);

            if (exists)
            {
                return Json($"اسم المستخدم '{userName}' مُستخدم بالفعل.");
            }

            return Json(true);
        }

        // ==========================================
        // 3. إضافة مستخدم جديد (Create)
        // ==========================================
        /// <summary>
        /// عرض صفحة الإدخال والنموذج الفارغ (GET: /User/Create)
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// استقبال بيانات النموذج وحفظها في قاعدة البيانات (POST: /User/Create)
        /// [ValidateAntiForgeryToken]: حماية النظام من هجمات تزوير الطلبات عبر المواقع (CSRF Attacks)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // فحص إضافي من جهة الخادم (Server-Side Validation) للأمان وموثوقية البيانات
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل.");
            }

            if (_context.Users.Any(u => u.userName == user.userName))
            {
                ModelState.AddModelError("userName", "اسم المستخدم مستخدم بالفعل.");
            }

            // فحص تحقق صحة النموذج وفق شروط الـ Data Annotations
            if (ModelState.IsValid)
            { 
                _context.Users.Add(user);          // إضافة الكائن إلى الذاكرة
                await _context.SaveChangesAsync();  // تنفيذ أمر INSERT في SQL Server وحفظ التغييرات نهائياً
                
                // TempData: رسالة مؤقتة تعيش لطلب واحد وتنتقل للصفحة التالية لتنبيه المستخدم بنجاح العملية
                TempData["Success"] = "تم قيد واعتمد العضو في الديوان الملكي بنجاح.";
                return RedirectToAction(nameof(Index)); // التوجيه لصفحة العرض بعد نجاح الحفظ
            }

            // إذا كانت هناك أخطاء في المدخلات، نعيد الصفحة مع نفس البيانات لتصحيحها
            return View(user);
        }

        // ==========================================
        // 4. تعديل بيانات المستخدم (Edit / Update)
        // ==========================================
        /// <summary>
        /// عرض صفحة تعديل المستخدم وجلب بياناته بناءً على الـ ID (GET: /User/Edit/5)
        /// </summary>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // إرجاع خطأ 404 في حال عدم وجود المستخدم
            }

            return View(user);
        }

        /// <summary>
        /// حفظ التعديلات الجديدة في قاعدة البيانات (POST: /User/Edit)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User model)
        {
            // التأكد من أن البريد الجديد غير مستخدم من قبل شخص آخر
            bool emailExists = _context.Users.Any(u => u.Email == model.Email && u.Id != model.Id);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل لحساب آخر.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(model);          // وسم الكائن بأنه معدل (EntityState.Modified)
                _context.SaveChanges();          // تنفيذ أمر UPDATE في قاعدة البيانات
                TempData["Success"] = "تم تحديث بيانات العضو بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // ==========================================
        // 5. حذف المستخدم (Delete)
        // ==========================================
        /// <summary>
        /// عرض صفحة تأكيد الحذف وتفاصيل العضو قبل حذفه (GET: /User/Delete/5)
        /// </summary>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        /// <summary>
        /// تنفيذ الحذف الفعلي بعد تأكيد المستخدم (POST: /User/Delete/5)
        /// [ActionName("Delete")]: يسمح بتغيير اسم الدالة مع الحفاظ على مسار التوجيه نفسه
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);        // وسم السجل للحذف
                await _context.SaveChangesAsync();  // تنفيذ أمر DELETE في SQL Server
                TempData["Success"] = "تم شطب قيد العضو من السجل بنجاح.";
            }
            catch 
            {
                TempData["Error"] = "تعذر الحذف لوجود ارتباطات متعلقة بهذا السجل.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 6. تسجيل الدخول (Authentication / Login)
        // ==========================================
        /// <summary>
        /// عرض واجهة تسجيل الدخول (GET: /User/Login)
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// معالجة بيانات الدخول والتحقق من صحة البريد وكلمة المرور (POST: /User/Login)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // الاستعلام عن وجود تطابق بين البريد وكلمة المرور في قاعدة البيانات
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "بيانات الدخول غير صحيحة (تأكد من البريد أو كلمة المرور).");
                    return View(model);
                }

                // عند نجاح الدخول يتم توجيه المستخدم لصفحة العرض الرئيسية
                TempData["Success"] = $"أهلاً بك مجدداً يا {user.userName} في الديوان الملكي.";
                return RedirectToAction("Index", "User");
            }

            return View(model);
        }
    }
}

