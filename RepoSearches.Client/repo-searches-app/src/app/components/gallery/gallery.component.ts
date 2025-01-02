import { Component, Input, forwardRef } from '@angular/core';
import { Repository } from 'src/app/models/repository';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => GalleryComponent),
      multi: true
    }
  ]

})
export class GalleryComponent {
repositories: Repository[] = [];

private onChange: (value: Repository[]) => void = () => {};
private onTouched: () => void = () => {};

writeValue(value: Repository[]): void {
  console.log('writeValue', value);
  this.repositories = value || [];
}

registerOnChange(fn: any): void {
  console.log('registerOnChange', fn);
  this.onChange = fn;
}

registerOnTouched(fn: any): void {
  console.log('registerOnTouched', fn);
  this.onTouched = fn;
}

// Example of triggering the callbacks
updateRepositories(newValue: Repository[]): void {
  this.repositories = newValue;
  this.onChange(this.repositories); // Notify parent form
}

markAsTouched(): void {
  console.log('markAsTouched');
  this.onTouched(); // Notify touched state
}
}
