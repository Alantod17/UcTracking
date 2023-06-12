import { Attribute, Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';

@Directive({
  selector: '[uiImageLoader]'
})
export class UiImageLoaderDirective {

  constructor(@Attribute('loader') public loader: string, @Attribute('onErrorSrc') public onErrorSrc: string, private renderer: Renderer2, private el: ElementRef) {
    if (!this.loader) this.loader = "https://img1.picmix.com/output/stamp/normal/8/5/2/9/509258_fb107.gif";
    if (!this.onErrorSrc) this.onErrorSrc = "https://img1.picmix.com/output/stamp/normal/8/5/2/9/509258_fb107.gif";
    this.renderer.setAttribute(this.el.nativeElement, 'src', this.loader);
  }

  @HostListener('load') onLoad() {
    this.renderer.setAttribute(this.el.nativeElement, 'src', this.el.nativeElement.src);
  }
  @HostListener('error') onError() {
    this.renderer.setAttribute(this.el.nativeElement, 'src', this.onErrorSrc);
  }
}
