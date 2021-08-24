
Scripts
========================

Config
-------------------------
Project folder contains ***Chrome-bin.rar***. It's local version of chrome browser which is ***works approporiate on the remote web-server*** with those scripts.

Extract archive on the machine to `C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin` or some other directory, but be aware that `binary_location` in each script specified to approporiate folder. 

```python
chrome_options.binary_location = "C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin\\chrome.exe"
```

All the information about user-session contains in folder:
```python
chrome_options.add_argument("--user-data-dir=C:/User_Data/" + username) 
```

Build
-------------------------
To generate .exe file from .py use the following command:

```code
pyinstaller --onefile --script-name.py
```

Response
-------------------------


Scripts have the same structure of result model:
```json
{
    "result": "unique type of response for each script's job",
    "hasError": "true/false",
    "error": "full information about error"
}
```

Supported scripts
-------------------------
### auth.py
Creates the authorized session

**Input**:
*   username
*   password

### check-auth.py
Returns information is the session is alive

**Input**:
*   session name

### account-info.py
Returns information about account

**Input**:
*   session name
*   specified account name

**Example**:
```code
> account-info.exe mkyrpychnyk weddingdressesguide
```
```json
{
   "result":{
      "username":"weddingdressesguide",
      "fullName":"Wedding Dresses Gallery",
      "follow":187,
      "following":1913143,
      "postsCount":6369,
      "isPrivate":false,
      "profilePicUrl":"https://instagram.fiev8-2.fna.fbcdn.net/v/t51.2885-19/s150x150/159346873_279600000316159_8261286409409204843_n.jpg?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_ohc=vV6PYTRGXBIAX_EVTph&amp;edm=ABfd0MgBAAAA&amp;ccb=7-4&amp;oh=3a8fddab546dbeca3d80240362dcd2a2&amp;oe=612C41E0&amp;_nc_sid=7bff83",
      "profilePicUrlHD":"https://instagram.fiev8-2.fna.fbcdn.net/v/t51.2885-19/s320x320/159346873_279600000316159_8261286409409204843_n.jpg?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_ohc=vV6PYTRGXBIAX_EVTph&amp;edm=ABfd0MgBAAAA&amp;ccb=7-4&amp;oh=b1703a28e6c75064f5f4356c081eac40&amp;oe=612D2768&amp;_nc_sid=7bff83"
   },
   "hasError":"false"
}
```

### post-info.py
Returns information about user-post

**Input**:
*   session name
*   post id

**Example**:
```code
> post-info.exe mkyrpychnyk CS9wxkhjl6v
```
```json
{
   "result":{
      "id":"2647486668668296879",
      "type":"GraphSidecar",
      "displayUrl":"https://instagram.fiev8-2.fna.fbcdn.net/v/t51.2885-15/fr/e15/p1080x1080/240454314_954255181804781_6624539988222226930_n.jpg?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_cat=1&amp;_nc_ohc=T_ct-zNuz_8AX-mYJT4&amp;edm=AABBvjUBAAAA&amp;ccb=7-4&amp;oh=475480b5ae36216f3f674710afe4ae23&amp;oe=6127CD56&amp;_nc_sid=83d603",
      "isVideo":false,
      "tagged":[
         "moonlightbridal"
      ],
      "ownerUsername":"weddingdressesguide",
      "createdTimeStamp":1629825026,
      "commentsCount":7,
      "likesCount":1197,
      "children":[
         {
            "id":"2647486533787025967",
            "type":"GraphVideo",
            "displayUrl":"https://instagram.fiev8-2.fna.fbcdn.net/v/t51.2885-15/fr/e15/p1080x1080/240454314_954255181804781_6624539988222226930_n.jpg?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_cat=1&amp;_nc_ohc=T_ct-zNuz_8AX-mYJT4&amp;edm=AABBvjUBAAAA&amp;ccb=7-4&amp;oh=475480b5ae36216f3f674710afe4ae23&amp;oe=6127CD56&amp;_nc_sid=83d603",
            "isVideo":true,
            "tagged":[
               "moonlightbridal"
            ],
            "viewsCount":7471,
            "videoUrl":"https://instagram.fiev8-2.fna.fbcdn.net/v/t50.2886-16/240421081_894524314778469_128986396443429140_n.mp4?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_cat=109&amp;_nc_ohc=IVn9x-nuprEAX-ANRv5&amp;edm=AABBvjUBAAAA&amp;ccb=7-4&amp;oe=6127BB59&amp;oh=07c070eb1bed453f2a77e136163950b5&amp;_nc_sid=83d603"
         },
         {
            "id":"2647486664843173123",
            "type":"GraphImage",
            "displayUrl":"https://instagram.fiev8-2.fna.fbcdn.net/v/t51.2885-15/fr/e15/p1080x1080/240411263_359477025660715_5571134264551835731_n.jpg?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_cat=1&amp;_nc_ohc=f0kLk36wWM8AX9eEjfD&amp;edm=AABBvjUBAAAA&amp;ccb=7-4&amp;oh=a27133418d527691d321b43cdcb3f0c9&amp;oe=612C7E13&amp;_nc_sid=83d603",
            "isVideo":false,
            "tagged":[
               "moonlightbridal"
            ]
         },
         {
            "id":"2647486664826412349",
            "type":"GraphImage",
            "displayUrl":"https://instagram.fiev8-2.fna.fbcdn.net/v/t51.2885-15/fr/e15/p1080x1080/240404501_351280249997324_232458783629244125_n.jpg?_nc_ht=instagram.fiev8-2.fna.fbcdn.net&amp;_nc_cat=108&amp;_nc_ohc=Gzz74-BLrZoAX_8hTkM&amp;edm=AABBvjUBAAAA&amp;ccb=7-4&amp;oh=46c8b0cd6b163259fbf853bad55093d6&amp;oe=612BD2F4&amp;_nc_sid=83d603",
            "isVideo":false,
            "tagged":[
               "moonlightbridal"
            ]
         }
      ]
   },
   "hasError":"false"
}
```

### user-posts.py
Returns information about account-posts

**Input**:
*   session name
*   specified account name
*   posts count
```code
> user-posts.exe mkyrpychnyk weddingdressesguide 10
```
```json
{
   "result":[
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS9wxkhjl6v",
         "postId":"CS9wxkhjl6v",
         "orderNumber":0
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS9XqdRrnxc",
         "postId":"CS9XqdRrnxc",
         "orderNumber":1
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS7mCwsLLl7",
         "postId":"CS7mCwsLLl7",
         "orderNumber":2
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS7VhLqroNv",
         "postId":"CS7VhLqroNv",
         "orderNumber":3
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS7BfGsLFLp",
         "postId":"CS7BfGsLFLp",
         "orderNumber":4
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS6yqxyrtDb",
         "postId":"CS6yqxyrtDb",
         "orderNumber":5
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS4sgwGLeF2",
         "postId":"CS4sgwGLeF2",
         "orderNumber":6
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS4NjNvrdOP",
         "postId":"CS4NjNvrdOP",
         "orderNumber":7
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS2cPNTrAOD",
         "postId":"CS2cPNTrAOD",
         "orderNumber":8
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS2CY96NvT-",
         "postId":"CS2CY96NvT-",
         "orderNumber":9
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CS1ookILYaB",
         "postId":"CS1ookILYaB",
         "orderNumber":10
      },
      {
         "ownerUsername":"weddingdressesguide",
         "postLink":"https://www.instagram.com/p/CSz3zGYL7Tc",
         "postId":"CSz3zGYL7Tc",
         "orderNumber":11
      }
   ],
   "hasError":"false"
}
```
