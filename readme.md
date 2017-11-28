SimpleMatchQueue
====

Straightforward match queue server for in-house testing purpose.


```
GET /queue/join
```
* __RequestParameters__
  * __suffix(optional)__ : client uniq id

* __Response__
  * __description__ : human readable string
  * __match_created__ : whether match created or not
  * __opponent__ : only valid if `match_created` is `true`
    * __uid__ : opponent's uniq id
    * __ip_address__ : opponent's ip address
  * __server_time__
