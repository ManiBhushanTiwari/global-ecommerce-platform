import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingHistoryComponent } from './shipping-history.component';

describe('ShippingHistoryComponent', () => {
  let component: ShippingHistoryComponent;
  let fixture: ComponentFixture<ShippingHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShippingHistoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ShippingHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
