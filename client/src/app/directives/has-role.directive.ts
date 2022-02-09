import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  user: User;

  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
  }

  ngOnInit(): void {
    if (!this.user || !this.user.roles) {
      return this.viewContainerRef.clear();
    }

    if (!this.user.roles.some((role) => this.appHasRole.includes(role))) {
      return this.viewContainerRef.clear();
    }

    this.viewContainerRef.createEmbeddedView(this.templateRef);
  }
}
