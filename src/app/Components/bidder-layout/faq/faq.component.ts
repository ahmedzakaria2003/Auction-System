import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FooterComponent } from "../../../Shared/footer/footer.component";
import { NavBarComponent } from "../../../Shared/nav-bar/nav-bar.component";

interface FAQItem {
  id: number;
  question: string;
  answer: string;
  category: string;
  isOpen: boolean;
}


@Component({
  selector: 'app-faq',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, FooterComponent, NavBarComponent],
  templateUrl: './faq.component.html',
  styleUrl: './faq.component.css'
})
export class FAQComponent {
searchTerm: string = '';
  selectedCategory: string = 'all';
  faqItems: FAQItem[] = [];
  filteredFAQs: FAQItem[] = [];

  ngOnInit() {
    this.loadFAQData();
    this.filteredFAQs = this.faqItems;
  }

  loadFAQData() {
    this.faqItems = [
      {
        id: 1,
        question: 'كيف يمكنني التسجيل في الموقع؟',
        answer: 'يمكنك التسجيل بسهولة من خلال النقر على زر "إنشاء حساب" واختيار نوع الحساب (مزايد أو بائع) وملء البيانات المطلوبة. ستحتاج إلى تأكيد بريدك الإلكتروني لتفعيل الحساب.',
        category: 'account',
        isOpen: false
      },
      {
        id: 2,
        question: 'ما هو ضمان المشاركة وكيف يعمل؟',
        answer: 'ضمان المشاركة هو مبلغ 50 جنيه مصري يجب دفعه قبل المشاركة في أي مزاد. هذا المبلغ يضمن جدية المزايدين ويتم استرداده في حالة عدم الفوز بالمزاد. إذا فزت بالمزاد ولم تدفع المبلغ المستحق، سيتم خصم الضمان.',
        category: 'bidding',
        isOpen: false
      },
      {
        id: 3,
        question: 'كيف يتم الدفع بعد الفوز بالمزاد؟',
        answer: 'بعد الفوز بالمزاد، ستتلقى إشعاراً لإتمام عملية الدفع خلال 48 ساعة. يمكنك الدفع باستخدام بطاقة الائتمان أو الخصم المباشر من خلال نظام Stripe الآمن. بعد تأكيد الدفع، سيتم التواصل معك لترتيب استلام المنتج.',
        category: 'payment',
        isOpen: false
      },
      {
        id: 4,
        question: 'هل يمكنني إلغاء مزايدتي؟',
        answer: 'لا يمكن إلغاء المزايدة بعد وضعها، لذا يرجى التأكد من رغبتك في المنتج والمبلغ قبل المزايدة. هذا يضمن عدالة المزاد لجميع المشاركين.',
        category: 'bidding',
        isOpen: false
      },
      {
        id: 5,
        question: 'كيف يمكنني بيع منتجاتي؟',
        answer: 'إذا كان لديك حساب بائع، يمكنك إضافة منتجاتك من لوحة التحكم الخاصة بك. ستحتاج إلى إضافة صور عالية الجودة، وصف مفصل، وتحديد السعر الابتدائي ومدة المزاد. سيتم مراجعة المنتج قبل نشره.',
        category: 'selling',
        isOpen: false
      },
      {
        id: 6,
        question: 'ما هي رسوم البيع؟',
        answer: 'نتقاضى عمولة 5% من قيمة البيع النهائية فقط في حالة بيع المنتج. لا توجد رسوم لإدراج المنتجات أو في حالة عدم البيع.',
        category: 'selling',
        isOpen: false
      },
      {
        id: 7,
        question: 'كيف يمكنني تغيير كلمة المرور؟',
        answer: 'يمكنك تغيير كلمة المرور من خلال الذهاب إلى الإعدادات في حسابك، ثم اختيار "تغيير كلمة المرور". ستحتاج إلى إدخال كلمة المرور الحالية والجديدة.',
        category: 'account',
        isOpen: false
      },
      {
        id: 8,
        question: 'ماذا يحدث إذا لم أدفع بعد الفوز؟',
        answer: 'إذا لم تقم بالدفع خلال 48 ساعة من انتهاء المزاد، سيتم خصم ضمان المشاركة وقد يتم تعليق حسابك مؤقتاً. المنتج سيُعرض على المزايد التالي أو يُعاد طرحه للمزاد.',
        category: 'payment',
        isOpen: false
      },
      {
        id: 9,
        question: 'كيف يمكنني التواصل مع البائع؟',
        answer: 'يمكنك التواصل مع البائع من خلال نظام الرسائل الداخلي في الموقع بعد الفوز بالمزاد. لا نسمح بمشاركة معلومات الاتصال المباشرة لضمان الأمان.',
        category: 'bidding',
        isOpen: false
      },
      {
        id: 10,
        question: 'هل المنتجات مضمونة؟',
        answer: 'جميع البائعين معتمدين ومراجعين من قبلنا. كما يمكنك مراجعة تقييمات البائع قبل المزايدة. في حالة وجود مشكلة مع المنتج، يمكنك التواصل مع خدمة العملاء خلال 7 أيام من الاستلام.',
        category: 'selling',
        isOpen: false
      }
    ];
  }

  toggleFAQ(id: number) {
    const faq = this.filteredFAQs.find(f => f.id === id);
    if (faq) {
      faq.isOpen = !faq.isOpen;
    }
  }

  filterByCategory(category: string) {
    this.selectedCategory = category;
    this.filterFAQs();
  }

  filterFAQs() {
    let filtered = this.faqItems;

    // Filter by category
    if (this.selectedCategory !== 'all') {
      filtered = filtered.filter(faq => faq.category === this.selectedCategory);
    }

    // Filter by search term
    if (this.searchTerm.trim()) {
      const searchLower = this.searchTerm.toLowerCase();
      filtered = filtered.filter(faq => 
        faq.question.toLowerCase().includes(searchLower) ||
        faq.answer.toLowerCase().includes(searchLower)
      );
    }

    this.filteredFAQs = filtered;
  }

  clearSearch() {
    this.searchTerm = '';
    this.selectedCategory = 'all';
    this.filteredFAQs = this.faqItems;
  }

  getCategoryIcon(category: string): string {
    const icons: { [key: string]: string } = {
      'bidding': 'fas fa-gavel',
      'payment': 'fas fa-credit-card',
      'account': 'fas fa-user',
      'selling': 'fas fa-store'
    };
    return icons[category] || 'fas fa-question';
  }


}
