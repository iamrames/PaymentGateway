import { Component } from '@angular/core';
import { FileResponse, PaymentCommand, PaymentsClient } from '../web-api-client';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public response: FileResponse;
  constructor(private client: PaymentsClient) {
    
  }

  get_payments(id: string) {
    this.client.payments_GetPayment(id).subscribe({
      next: result => this.response = result,
      error: error => console.error(error)
    });
  }

  create_payments(command: PaymentCommand) {
    this.client.payments_CreatePayment(command).subscribe({
      next: result => this.response = result,
      error: error => console.error(error)
    });
  }
}
