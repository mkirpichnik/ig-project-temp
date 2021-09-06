import { TestBed } from '@angular/core/testing';

import { PostsHistoryService } from './posts-history.service';

describe('PostsHistoryService', () => {
  let service: PostsHistoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostsHistoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
