
## Third part bot webhook
  - [Visitor message sent](#visitor-message-sent)
  - [Intent link clicked](#Intent-link-clicked)
  - [Helpful or not-helpful clicked](#helpful-or-not-helpful-clicked)
  - [Location Collected](#location-collected)
  - [Information Collected](#information-collected)
### Third part bot webhook Related Object Json Format

#### VisitorInfo

  Visitor info is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `id` | integer | yes | no | id of the visitor |
  | `longitude` | float | no | no | longitude of the visitor location |
  | `latitude` | float | no | no | latitude of the visitor location |
  | `page_views` | integer | no | yes | count of the visited |
  | `browser` | string | no | yes | visitor use browser type |
  | `chats` | integer | no | yes | count of chat |
  | `city` | string | no | yes | the city of the visitor |
  | `company` | string | no | yes | the company of the visitor |
  | `country` | string | no | yes | the country of the visitor |
  | `current_browsing` | string | no | yes | page of the current browsing |
  | `custom_fields` | [CustomFields](#customfields) | no | yes | an array of custom fields |
  | `custom_variables` | [CustomVariables](#customvariables) | no | yes | an array of custom variables |
  | `department` | int | no | yes | department of the visitor |
  | `email` | string | no | yes | email of the visitor |
  | `first_visit_time` | string | no | yes | the time of first visit |
  | `flash_version` | string | no | yes | version of the flash |
  | `ip` | string | no | yes | ip of the visitor |
  | `keywords` | string | no | yes | search engine key |
  | `landing_page` | string | no | yes | the page of login |
  | `language` | string | no | yes | language |
  | `name` | string | no | yes | name of the visitor |
  | `operating_system` | string | no | yes | operating system of the visitor |
  | `phone` | string | no | yes | phone of the visitor |
  | `product_service` | string | no | yes | product service |
  | `referrer_url` | string | no | yes | referrer url |
  | `screen_resolution` | string | no | yes | screen resolution |
  | `search_engine` | string | no | yes | search engine |
  | `state` | string | no | yes | state of the visitor |
  | `status` | string | no | yes | status of the visitor |
  | `time_zone` | string | no | yes | time zone of the visitor |
  | `visit_time` | string | no | yes | time of the visitor |
  | `visits` | integer | no | yes | count of the visited |

#### CustomFields

  Custom fields is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `id` | integer  | yes | no | id of the field |
  | `name` | string  | no | yes | name of the field |
  | `value` | string  | no | yes | value of the field |

#### CustomVariables

  Custom variables is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `name` | string  | no | yes | name of the variable |
  | `value` | string  | no | yes | value of the variable |

#### ImageResponse
  ImageResponse is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `name` | string  | no | yes | name of the image |
  | `url` | string  | no | yes | url of the image |  

#### VideoResponse
  VideoResponse is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `url` | string  | no | yes | url of the video |

#### TextResponse
  TextResponse is represented as simple flat JSON objects with the following keys:

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `message` | string  | no | yes | text of the response |
  | [linkInfo](#linkinfo) | object  | no | no | link information of the text |  

#### LinkInfo
  TextResponse is represented as simple flat JSON objects with the following keys:

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `type` | enums | no | yes | enums contain weblink and goToIntent |
  | `startPos` | int | no | yes | start index of text which contains link info |
  | `endPos` | int | no | yes | end index of text which contains link info |
  | `url` | string | no | yes when type is weblink | url of the web resource that you want user to open,including web forms,articles,images,video,etc. |
  | `intentId` | string| no | yes when type is goToIntent | id of intent that you want user to click. |
  | `intentName` | string| no | yes when type is goToIntent | name of intent that you want user to click. |
  | `openIn` | enums | no | yes when type is weblink | enums contain currentWindow,sideWindow,newWindow. This field defined the way that webpage will be opened. |


#### QuickReplyResponse
  QuickReplyResponse is represented as simple flat JSON objects with the following keys:

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `message` | string  | no | yes | text of the response|
  | [quickReplyItems]| an array of [QuickReplyItem](#quickreplyitem)  | no | no | link information of the text|  

#### QuickReplyItem
  QuickReplyItem is represented as simple flat JSON objects with the following keys: 

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `type` | string  | no | yes | enums contain  goToIntent, contactAgent, text|
  | `name`| string  | no | yes | text on quick reply |
  | `intentId`| string  | no | yes when type is goToIntent  | id of the intent which current quickreply point to |
  | `intentName`| string  | no | yes when type is goToIntent  | name of the intent which current quickreply point to |  

#### ButtonResponse
  ButtonResponse is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `message` | string  | no | yes | text of the response|
  | [buttonItem]| an array of [ButtonItem](#buttonItem)  | no | no | link information of the text|  

#### ButtonItem
  QuickReplyResponse is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `type` | string  | no | yes | enums contain  enums contain weblink,webview and goToIntent|
  | `text`| string  | no | no | text on button |
  | `url` | string | no | yes when type is weblink or webview | url of the web resource that you want user to open,including web forms,articles,images,video,etc.|
  | `intentId`| string  | no | yes when type is goToIntent | id of the intent which current quickreply point to |
  | `intentName`| string  | no | yes when type is goToIntent | name of the intent which current quickreply point to |
  | `openIn` | enums | no | yes when type is weblink | enums contain currentWindow,sideWindow,newWindow. This field defined the way that webpage will be opened.|
  | `openStyle` | enums | no | yes when type is webview | enums contain compact, tall, full. This field defined the way that webview will be opened.|


#### CollectInformationResponse
  CollectInformationResponse is represented as simple flat JSON objects with the following keys:  
  
  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `text` | string  | no | yes | text on the button which can be clicked to open a webview to collection information|
  | `message` | string  | no | yes | message of the response which will be displayed upon the button|
  | `isNeedConfirm` | bool  | no | yes | whether need to confirm after webview submit|
  | `fields` | an array of [Field](#field)  | no | yes | fields displayed on webview|

#### Field

  Field is represented as simple flat JSON objects with the following keys:  
  
  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `Id` | int  | no | yes | id of the field |
  | `name` | string  | no | yes | name of the field|
  | `value` | string  | no | yes | value of the field|
  | `type` | string  | no | yes | field type, contains text, textArea, radio, checkBox, dropDownList, checkBoxList |
  | `isRequired` | bool  | no | yes | when it is true, visitor have to input a value in the field before submit |
  | `isMasked` | bool  | no | yes | when it is true, information collected will replaced by * in chat log for security |
  | `option` | an array of string  | no | yes when type is dropDownList, checkBoxList | values displayed in the field when type is dropDownList, checkBoxList for visitor to choose|

#### Response
Response is represented as simple flat json objects with the following keys:

|Name| Type| Read-only    |Mandatory | Description     | 
| - | - | - | - | - | 
|`type` | string | no | yes |enums contain text,image,video, quickreply, button, collectLocation, collectInformation.  | 
|`id` | string | no | yes |id of current response.  | 
|`content` | object | no | yes |response's content. when type is text, it represents [TextResponse](#textresponse);when type is image ,it represents [ImageResponse](#imageresponse);when type is video, it represents [VideoResponse](#videoresponse); when type is quickreply, it represents [QuickReplyResponse](#quickreplyresponse); when type is button, it represents [ButtonResponse](#buttonresponse); when type is collectLocation, it should be null; when type is collectInformation, it represents [CollectInformationResponse](#collectinformationresponse)| 


#### Visitor Message Sent

##### Request

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `question` - the last question that Bot receives from visitor
  - `questionId` - id of current question
  - [visitorInfo](#VisitorInfo)

##### Response
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)


#### Intent link clicked

##### Request

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of originall question
  - `intentId` - id of intent which visitor clicked,it is originally from the response of the webhook [Visitor Message Sent](#visitor-message-sent), another [Intent link clicked](#intent-link-clicked), [Location Collected](#location-collected), [Information Collection](#information-collected)
  - [visitorInfo](#VisitorInfo)

##### Response
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)


#### Location Collected

##### Request

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of originall question
  - `intentId` - id of intent which visitor clicked,it is originally from the response of the webhook [Visitor Message Sent](#visitor-message-sent), another [Intent link clicked](#intent-link-clicked), [Location Collected](#location-collected), [Information Collection](#information-collected)
  - [visitorInfo](#VisitorInfo)

##### Response
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)

#### Information Collected

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of originall question
  - `intentId` - id of intent which visitor clicked,it is originally from the response of the webhook [Visitor Message Sent](#visitor-message-sent), another [Intent link clicked](#intent-link-clicked), [Location Collected](#location-collected), [Information Collection](#information-collected)
  - [visitorInfo](#VisitorInfo)

##### Response
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)

#### Helpful or not-helpful clicked

##### Request

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of [response](#response),it is originally from the response of the webhook [Visitor Message Sent](#visitor-message-sent) or [Intent link clicked](#intent-link-clicked)
  - `isHelpful` - true or false
  - [visitorInfo](#VisitorInfo)

##### Response
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)