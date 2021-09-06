import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IPost } from "../dashboard/posts-dashboard/posts-dashboard.component";
import { IAccount } from "./models/account.model";

@Injectable({
  providedIn: 'root'
})
export class PostsHistoryService {
  constructor(private http: HttpClient) {}

  accounts(): Observable<IAccount[]> {
    return this.http.get<IAccount[]>(`/api/PostsHistory/accounts`);
  }

  accountPosts(accountName: string) {
    return this.http.get<IPost[]>(`/api/PostsHistory/account/${accountName}/posts`);
  }
}
