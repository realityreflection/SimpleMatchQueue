SimpleMatchQueue
====

Straightforward match queue server for in-house testing purpose.

```
GET /queue/join
```
* __RequestParameters__
  * suffix(optional) : client uniq id

* __Response__
  * description : human readable string
  * match_created : whether match created or not
  * opponent : only valid if `match_created` is `true`
    * uid : opponent's uniq id
    * ipAddress : opponent's ip address
  * server_time
