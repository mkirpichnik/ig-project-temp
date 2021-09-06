import { IAccount } from './../../core/models/account.model';
import { Component, OnInit } from '@angular/core';
import { PostsHistoryService } from 'src/app/core/posts-history.service';

@Component({
  selector: 'app-posts-dashboard',
  templateUrl: './posts-dashboard.component.html',
  styleUrls: ['./posts-dashboard.component.scss']
})
export class PostsDashboardComponent implements OnInit {

  accounts: IAccount[];

  posts: {
    [account:string]: IPost[]
  } = {};

  loading: boolean = true;

  constructor(private postsHistoryService: PostsHistoryService) { }

  async ngOnInit() {
    this.accounts = await this.postsHistoryService.accounts().toPromise();
    if (this.accounts) {
      for(const account of this.accounts) {
        this.postsHistoryService.accountPosts(account.username)
          .subscribe(result => {
            this.posts[account.username] = result;
            this.loading = false;
          })
      }
    }
  }

}

export interface IPost {
  postId: string;
  postLink: string;
  ownerUsername: string;
  postType: PostType;
  likesCount: number;
  commentsCount: number;
  createdDateTime: Date;
  lastUpdate: Date;
  content: IPostContent;
}

export interface IPostContent {
  id: string;
  contentUrl: string;
  tagged: string[];
  contentType: PostType;
  order: number;
}

export enum PostType {
  Photo = 'GraphImage',
  Video = 'GraphVideo',
  Carousel = 'GraphSidecar'
}
