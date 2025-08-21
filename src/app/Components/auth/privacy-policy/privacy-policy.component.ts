import { Component } from '@angular/core';

@Component({
  selector: 'app-privacy-policy',
  standalone: true,
  imports: [],
  templateUrl: './privacy-policy.component.html',
  styleUrl: './privacy-policy.component.css'
})
export class PrivacyPolicyComponent {

  acceptPrivacyPolicy() {
    // Close modal and emit acceptance event
    const modal = document.getElementById('privacyModal');
    if (modal) {
      const bootstrapModal = (window as any).bootstrap?.Modal?.getInstance(modal);
      if (bootstrapModal) {
        bootstrapModal.hide();
      }
    }
    
    // You can emit an event here to notify parent component
    // or handle the acceptance logic
    console.log('Privacy policy accepted');
  }

}
