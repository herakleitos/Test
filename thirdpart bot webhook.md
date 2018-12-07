
## Custom Bot webhook
  - [Visitor question sent](#visitor-question-sent)

When visitor sent a question through live chat, we will pass this question and other information we defined to this webhook. 
You need process this question and information within this webhook using your own bot engine and give us a formatted 
response so that we can give visitor an answer base on your response through live chat. 

  - [Intent link clicked](#Intent-link-clicked)

If the answer we give to visitor contains link/button/quickreply which point to an intent, when visitor click this link/button/quickreply, we will pass this action to this webhook with intent id and other information we defined. You need process this action within this webhook and give us a formatted response so than we can give an answer to visitor base on your response through live chat.

  - [Helpful or not-helpful clicked](#helpful-or-not-helpful-clicked)

Visitor can click helpful or not-helpful button to rate our answers. When visitor clicked these buttons, we will pass this action to this webhook, you can use this webhook to collect information about your bot’s correctness and improve your bot’s experience. Also, we need a formatted response from this webhook to give visitor a message for his/her rating.

  - [Location sent](#location-sent)

When we received a response whose type is collectLocation, we will display an webview for visitor to collect his/her location, when visitor shared his/her location to us, we will pass these information to this webhook and you can give us a response based on information we provided through this webhook.

  - [Information sent](#information-sent)

When we received a response whose type is collectInformation, we will display an webview for visitor to collect more information about him/her, when visitor filled out webview, we will pass these information to this webhook, and you can give us a response based on information we provided through this webhook.

### Visitor question Sent

#### Request data

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `question` - the last question that Bot receives from visitor
  - `questionId` - id of current question
  - [visitorInfo](#VisitorInfo)

#### Response data
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)
  
  [Sample Json](#sample-json)


### Intent link clicked

#### Request data

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of originall question
  - `intentId` - id of intent which visitor clicked,it is originally from the response of the webhook [Visitor question Sent](#visitor-question-sent), another [Intent link clicked](#intent-link-clicked), [Location Collected](#location-collected), [Information Collection](#information-collected)
  - [visitorInfo](#VisitorInfo)

#### Response data
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)

  [Sample Json](#sample-json)

### Helpful or not-helpful clicked

#### Request data

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of [response](#response),it is originally from the response of the webhook [Visitor question Sent](#visitor-question-sent) or [Intent link clicked](#intent-link-clicked)
  - `isHelpful` - true or false
  - [visitorInfo](#VisitorInfo)

#### Response data
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)

  [Sample Json](#sample-json)

### Location sent

#### Request data

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of originall question
  - `intentId` - id of intent which visitor clicked,it is originally from the response of the webhook [Visitor question Sent](#visitor-question-sent), another [Intent link clicked](#intent-link-clicked), [Location Collected](#location-collected), [Information Collection](#information-collected)
  - [visitorInfo](#VisitorInfo)

#### Response data
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)

  [Sample Json](#sample-json)

### Information sent

#### Request data

  - `sessionId ` -  id of the session
  - `campaignId` - id of the campaign in comm100 live chat
  - `questionId` - id of originall question
  - `intentId` - id of intent which visitor clicked,it is originally from the response of the webhook [Visitor question Sent](#visitor-question-sent), another [Intent link clicked](#intent-link-clicked), [Location Collected](#location-collected), [Information Collection](#information-collected)
  - [visitorInfo](#VisitorInfo)

#### Response data
  - `type` - string , contains  highConfidenceAnswer, possibleAnswer, noAnswer
  - `answer` - an array of [Response](#response)

  [Sample Json](#sample-json)

### Custom Bot webhook Related Object Json Format

#### Response
Response is represented as simple flat json objects with the following keys:

|Name| Type| Read-only    |Mandatory | Description     | 
| - | - | - | - | - | 
|`type` | string | no | yes |enums contain text,image,video, quickreply, button, collectLocation, collectInformation.  | 
|`id` | string | no | yes |id of current response.  | 
|`content` | object | no | yes |response's content. when type is text, it represents [TextResponse](#textresponse);when type is image ,it represents [ImageResponse](#imageresponse);when type is video, it represents [VideoResponse](#videoresponse); when type is quickreply, it represents [QuickReplyResponse](#quickreplyresponse); when type is button, it represents [ButtonResponse](#buttonresponse); when type is collectLocation, it should be null; when type is collectInformation, it represents [CollectInformationResponse](#collectinformationresponse)| 

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

#### QuickReplyResponse
  QuickReplyResponse is represented as simple flat JSON objects with the following keys:

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `message` | string  | no | yes | text of the response|
  | `quickReplyItems`| an array of [QuickReplyItem](#quickreplyitem)  | no | no | link information of the text|  

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
  | `buttonItems`| an array of [ButtonItem](#buttonItem)  | no | no | link information of the text|  

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

#### VisitorInfo

  Visitor info is represented as simple flat JSON objects with the following keys:  

  | Name | Type | Read-only | Mandatory | Description |    
  | - | - | - | - | - | 
  | `id` | integer | yes | no | id of the visitor |
  | `chats` | integer | no | yes | count of chat |
  | `name` | string | no | yes | name of the visitor |
  | `language` | string | no | yes | language |
  | `email` | string | no | yes | email of the visitor |
  | `phone` | string | no | yes | phone of the visitor |
  | `longitude` | float | no | no | longitude of the visitor location |
  | `latitude` | float | no | no | latitude of the visitor location |
  | `department` | int | no | yes | department of the visitor |
  | `company` | string | no | yes | the company of the visitor |
  | `city` | string | no | yes | the city of the visitor |
  | `country` | string | no | yes | the country of the visitor |
  | `browser` | string | no | yes | visitor use browser type |
  | `page_views` | integer | no | yes | count of the visited |
  | `current_browsing` | string | no | yes | page of the current browsing |
  | `referrer_url` | string | no | yes | referrer url |
  | `landing_page` | string | no | yes | the page of login |
  | `search_engine` | string | no | yes | search engine |
  | `keywords` | string | no | yes | search engine key |
  | `operating_system` | string | no | yes | operating system of the visitor |
  | `ip` | string | no | yes | ip of the visitor |
  | `flash_version` | string | no | yes | version of the flash |
  | `product_service` | string | no | yes | product service |
  | `screen_resolution` | string | no | yes | screen resolution |
  | `time_zone` | string | no | yes | time zone of the visitor |
  | `first_visit_time` | string | no | yes | the time of first visit |
  | `visit_time` | string | no | yes | time of the visitor |
  | `visits` | integer | no | yes | count of the visited |
  | `state` | string | no | yes | state of the visitor |
  | `status` | string | no | yes | status of the visitor |
  | `custom_fields` | [CustomFields](#customfields) | no | yes | an array of custom fields |
  | `custom_variables` | [CustomVariables](#customvariables) | no | yes | an array of custom variables |

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

  #### Sample Json
  ```json
  {
    "type": "highConfidenceAnswer",
    "answer": [
        {
            "id": "1",
            "type": "text",
            "content": {
                "message": "this is a plain message"
            }
        },
        {
            "id": "2",
            "type": "text",
            "content": {
                "message": "this is a web link message",
                "linkInfo": {
                    "type": "weblink",
                    "startPos": 10,
                    "endPos": 17,
                    "url": "www.test.com",
                    "openIn": "currentWindow"
                }
            }
        },
        {
            "id": "3",
            "type": "text",
            "content": {
                "message": "this is a go to intent message",
                "linkInfo": {
                    "type": "weblink",
                    "startPos": 10,
                    "endPos": 17,
                    "intentId": "test-intent-id",
                    "intentName": "test-intent-name",
                    "openIn": "currentWindow"
                }
            }
        },
        {
            "id": "4",
            "type": "image",
            "content": {
                "name": "test-image.jpg",
                "url": "www.test.com/test-image.jpg"
            }
        },
        {
            "id": "5",
            "type": "video",
            "content": {
                "url": "www.test.com/test-video.jpg"
            }
        },
        {
            "id": "6",
            "type": "quickreply",
            "content": {
                "message": "this is a quick reply response",
                "quickReplyItems": [
                    {
                        "type": "goToIntent",
                        "name": "click to trigger test-intent-name",
                        "intentId": "test-intent-id",
                        "intentName": "test-intent-name"
                    },
                    {
                        "type": "contactAgent",
                        "name": "click to contact agent"
                    },
                    {
                        "type": "text",
                        "name": "click to send this text"
                    }
                ]
            }
        },
        {
            "id": "7",
            "type": "button",
            "content": {
                "message": "this is a button response",
                "buttonItems": [
                    {
                        "type": "goToIntent",
                        "text": "click to trigger test-intent-name",
                        "intentId": "test-intent-id",
                        "intentName": "test-intent-name"
                    },
                    {
                        "type": "weblink",
                        "text": "click to open this url in web page",
                        "url": "www.test.com",
                        "openIn": "currentWindow"
                    },
                    {
                        "type": "webview",
                        "text": "click to open this url in web view",
                        "url": "www.test.com",
                        "openStyle": "full"
                    }
                ]
            }
        },
        {
            "id": "8",
            "type": "collectLocation",
            "content": null
        },
        {
            "id": "9",
            "type": "collectInformation",
            "content": {
                "text": "",
                "message": "",
                "isNeedConfirm": "true",
                "fields": [
                    {
                        "Id": 1,
                        "name": "field-1",
                        "value": "",
                        "type": "text",
                        "isRequired": true,
                        "isMasked": true
                    },
                    {
                        "Id": 2,
                        "name": "field-2",
                        "value": "",
                        "type": "dropDownList",
                        "isRequired": true,
                        "isMasked": true,
                        "option": [
                            "value-1",
                            "value-2",
                            "value-3"
                        ]
                    }
                ]
            }
        }
    ]
}
