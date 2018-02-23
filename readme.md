`SimpleMatchQueue
====

Straightforward match queue server for in-house testing purpose.

Flow
----
* Client starts as `Host` (Please make sure that you are ready to accept peers)
* Client sends `/queue/join` every 1 seconds.
* If `match_created` == true, then connect to `opponent`. (Stop hosting before making a connection)
* If you want to quit from match queue, just stop calling `/queue/join`

Method
----
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
