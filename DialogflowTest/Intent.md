### Intent
  +  `POST /api/v1/bot/bots/{botId}/intents`
  +  `PUT /api/v1/bot/bots/{botId}/intents`
  +  `GET /api/v1/bot/bots/{botId}/intents`
  +  `GET /api/v1/bot/bots/{botId}/intents/{intentId}`

### Secure Form Json Format
Secure Form is represented as simple flat JSON objects with the following keys:  

|name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id |integer  | yes | no |id of the secure form.
|name| string  | no | yes |  name of the secure form.
|description| string  | no | no | description of the secure form.when entityCollectionType is viaForm, this is a message that will be sent before the button.when entityCollectionType is viaForm, this is a message that will be sent before the button.
|fields| Array | no | no |an array of [field](#field) object.

#### IntentBase Json Format
IntentBase is represented as simple flat json objects with the following keys: 

|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------ 
|id | integer | yes | yes except create |id of the intent.
|siteId | integer  | yes | no |id of the current site.
|botId | integer  | yes | no |id of the current bot.
|intentName | string | no | yes | name of the intent.
|categoryId| integer | no | yes | value of the custom field.
|ifRequireDetailInfo | bool | no | no | whether need visitor to provide more detail information.
|entityCollectionType | string | yes if ifRequireDetailInfo is true | no | enums contain viaForm and viaPrompts,this represents the way you want to collect  visitor's information. there are two options: viaForm and viaPrompts.
|formMessage | string | no | yes if entityCollectionType is viaForm | when entityCollectionType is viaForm, this is a message that will be sent before the button.
|formTitle | string | no | yes if entityCollectionType is viaForm | when entityCollectionType is viaForm,a button will sent to visitor if bot need to collect detail information,visitor can click this button to open the form to fillout information ,this is the text on this button.and also this is the title of that form.
|ifRequireConfirm | bool | no | yes | whether need visitor to confirm after collect all detail information that bot needed.
|ifRequireLocation | bool | no | yes | whether need to collect visitor's location information.

#### IntentSignInSettings Json Format
IntentSignInSettings is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------ 
|id | integer  | yes | no |id of the current item.
|channelType | string  | no | yes |enums contain default,livechat,facebook and twitter.
|signInMessage | string  | no | yes |text sent to visitor before signin in button.
|signInLinkText | string  | no | yes |text on signin button.
|isSSO | string  | no | yes |whether is single sign on.
|signInURL | string  | no | yes |url of the signin page.
|customVariable | string  | no | yes |custom value sent to signin page.
|openIn | string  | no | yes |enums contain sideWindow,newWindow,currentWindow,when channelType is livechat, it represents the way that a page will be opened.

#### Questions Json Format
Questions is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------ 
|id | integer  | yes | no |id of the current item.
|question | string  | no | yes |question you can expect from users,that will trigger this intent.
|questionEntities | array  | no | yes |an array of [QuestionsEntities](#QuestionsEntities-Json-Format) that you want to mark on current question.

#### QuestionsEntities Json Format
QuestionsEntities is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------ 
|id | integer  | yes | no |id of the current item.
|startPos | integer | no | yes |strat index  of current question you marked.
|endPos | integer | no | yes |end index  of current question you marked.
|entityId | integer | no | yes |id of entity marked on one question.
|entityLabel | string | no | yes |label to distinguish same entity marked on one question.

#### EntityCollectionFormFields Json Format
EntityCollectionFormFields is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|fieldType | string | no | yes |enums contain test ,testArea,radioBox ,checkBox ,dropDownList ,checkBoxList, this is the type of fields appear on the form.
|id | integer  | yes | no |id of the current item.
|fieldName | string | no | yes |this is the field's name appear on the form.
|entityId | integer | no | yes |id of entity marked on one question.
|entityLabel | string | no | yes |label to distinguish same entity marked on one question.
|isRequired | bool | no | yes |it marks whether the field appear on the form is required or not.
|isMasked | bool | no | yes |if this is true,visitor's information will replaced by anonymous symbol in chat logs.
|options | array | no | no |an array of [PromptsOption](#PromptsOption-Json-Format).
|orderNumber | integer | no | yes |sequence of this field.

#### EntityCollectionPrompts Json Format
EntityCollectionPrompts is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|entityId | integer | no | yes |id of entity marked on one question.
|entityLabel | string | no | yes |label to distinguish same entity marked on one question.
|prompts | array | no | yes |an array of [PromptQuestion](#PromptQuestion-Json-Format).
|options | array | no | no |an array of [FormFieldsOption](#FormFieldsOption-Json-Format).
|orderNumber | integer | no | yes |sequence of this item.

#### PromptsOption Json Format
PromptsOption is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|option | string | no | yes |this will be sent to visitor as quick reply below prompt.

#### FormFieldsOption Json Format
PromptsOption is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|option | string | no | yes |this will be added to fields such as dropdownlist,checkboxlist.

#### PromptQuestion Json Format
PromptQuestion is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    | Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|prompt | string | no | yes |this is a question that bot will ask visitors,if a request doesn't contain current entity.

#### TextResponse Json Format
TextResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|responseTexts | array | no | yes |an array of [ResponseText](#ResponseText-Json-Format).

#### ResponseText Json Format
ResponseTexts is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|text | string | no | yes |enums contain default,livechat,facebook and twitte
|orderNumber | integer | no | yes |sequence of current item.

#### ImageResponse Json Format
ImageResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|name | string | no | yes |name of the image you choosed.
|imageUrl | string | no | yes |url of the image you choosed.

#### VideoResponse Json Format
VideoResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|videoUrl | string | no | yes |url of the video you choosed.

#### WebhookResponse Json Format
WebhookResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|webhookUrl | string | no | yes |url of the webhook.

#### ComplexResponse Json Format
ComplexResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|complexText | string | no | yes |html text updated from old data.

#### QuickReplyResponse Json Format
QuickReplyResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|responseText | string | no | yes |text sent before quickreplys.
|quickReplyId | integer | no | yes |id of quickreply you choosed.

#### ButtonResponse Json Format
ButtonResponse is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|responseText | string | no | yes |text above buttons,this text will be sent before buttons.
|buttons | array | no | yes |an array of [Button](#Button-Json-Format).

#### Button Json Format
Button is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|id | integer  | yes | no |id of the current item.
|buttonText | string | no | yes |text on button.
|buttonType | string | no | yes |enums contain goToIntent,link and webView,type of buttons
|linkUrl | string | no | yes if buttonType is link or webView|url of the web page you want to open.
|goTointentId | string | no | yes if buttonType is goToIntent | id of the intent you choosed.
|goTointentName | string | no | yes if buttonType is goToIntent | the name of the intent you choosed.
|openIn | string | no | yes if channelType is livechat and buttonType is link or webView |enums contain sideWindow,newWindow,currentWindow, it represents the way that a page will be opened.
|openStyle | string | no | yes if channelType is livechat or facebook and buttonType is webView |enums contain compact,tall and full,it represents the size of the webview that will be opened.
|orderNumber | integer | no | yes |sequence of current item.

#### Response Json Format
Response is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|type | string | no | yes |enums contain text,image,video,webhook,button,quickReply,complex.
|content | json object | no | yes |response's content. when type is text, it represents [TextResponse](#TextResponse-Json-Format);when type is image ,it represents [ImageResponse](#ImageResponse-Json-Format);when type is video, it represents [VideoResponse](#VideoResponse-Json-Format); when type is webhook,it represents [WebhookResponse](#WebhookResponse-Json-Format);when type is button,it represents [ButtonResponse](#ButtonResponse-Json-Format);when type is quickReply, it represents [QuickReplyResponse](#QuickReplyResponse-Json-Format);when type is complex,it represents [ComplexResponse](#ComplexResponse-Json-Format).
|orderNumber | string | no | yes|sequence of current item.

#### AnswerSubItem Json Format
AnswerSubItem is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|response| aray| no |no | an array of [Response](#Response-Json-Format)
|isNeedSignInBeforeBotRespond| bool| no |yes | whether need sign in when bot response visitor's question   
|intentSignInSettings| json object| no |yes if isNeedSignInBeforeBotRespond is true | an item of [IntentSignInSettings](#IntentSignInSettings-Json-Format)

#### Answer Json Format
Answer is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|default| json object| no |no | an json object of [AnswerSubItem](#AnswerSubItem-Json-Format),but AnswerSubItem.response.type can not be image,video,webhook,complex.
|livechat| json object| no |no | an json object of [AnswerSubItem](#AnswerSubItem-Json-Format).
|facebook| json object| no |no | an json object of [AnswerSubItem](#AnswerSubItem-Json-Format),but AnswerSubItem.response.type can not be complex.
|twitter| json object| no |no | an json object of [AnswerSubItem](#AnswerSubItem-Json-Format),but AnswerSubItem.response.type can not be complex.

#### Intent Json Format
Intent is represented as simple flat json objects with the following keys:
|Name| Type| Read-only    |Mandatory | Description   
| ------------- |--------------------- | ---------- | -------------------- | ------------------
|intentBase| json object| no |yes | an json object of [IntentBase](#IntentBase-Json-Format).
|questions| array| no |yes | an array of [Questions](#Questions-Json-Format).
|entityCollectionFormFields| array| no |yes if intentBase.entityCollectionType is viaForm | an array of [EntityCollectionFormFields](#EntityCollectionFormFields-Json-Format).
|entityCollectionPrompts| array| no |yes if intentBase.entityCollectionType is viaPrompts | an array of [EntityCollectionPrompts](#EntityCollectionPrompts-Json-Format).
|answer| json object| no |yes | an item of [Answer](#Answer-Json-Format).

#### Create a new intent
- End Point 
  `POST /api/v1/bot/bots/{botId}/intents`
- Parameters

   path parameters

  + `botId` - required , id of current bot

   request body parameters
  + `learningQuestionId` - int,id from visitor's  not matched  questions,optional
  + `intent` - an item of [Intent](#Intent-Json-Format),required

- Response

   when the http status Code is 200, the response is as below:
  + `intent` - an item of [Intent](#Intent-Json-Format),required

  the other case, the http status Code maybe:
  + `500`  -server internal error. when the http status code is 500, there may have detailled error message.
  + `400`  -bad request. when the http status code is 400, there will have detailled error message.
  + `401`  -unauthorized
  + `404`  -not found

#### Edit a new intent
- End Point 

  `PUT /api/v1/bot/bots/{botId}/intents`
- Parameters

  path parameters
  + `botId` - required , id of current bot

  request body parameter
  + `learningQuestionId` - int,optional,id from visitor's  not matched  questions
  + `intent` - an item of [Intent](#Intent-Json-Format),required

- Response

   when the http status code is 200, the response is as below:
  + `intent` - an item of [Intent](#Intent-Json-Format),required

   the other case, the http status code maybe:
  + `500`  -server internal error. when the http status code is 500, there may have detailled error message.
  + `400`  -bad request. when the http status code is 400, there will have detailled error message.
  + `401`  -unauthorized
  + `404`  -not found

####  Get a intent
- End Point

   `GET /api/v1/bot/bots/{botId}/intents/{intentId}`

- Parameter

  path parameters
  + `botId` - id of current bot,required
  + `intentId` - id of the intent you want to get,required

- Response

   when the http status code is 200, the response is as below:

  + `intent` - an item of [Intent](#Intent-Json-Format)

   the other case, the http status code maybe:
  + `500`  -server internal error. when the http status code is 500, there may have detailled error message.
  + `400`  -bad request. when the http status code is 400, there will have detailled error message.
  + `401`  -unauthorized
  + `404`  -not found

#### Get intent by category or by intent name/question
- End Point

   `GET /api/v1/bot/bots/{botId}/intents`

- Parameters

   path parameters
  + `botId` - id of current bot,required

  query parameters

  + `categoryId` -id of the category you want to explorer
  + `nameOrQuestion` -name or question of the intent you want to explorer

- Response

   when the http status code is 200, the response is as below:

  +  `intentBase` - an array of [IntentBase](#IntentBase-Json-Format)

   the other case, the http status code maybe:
  + `500`  -server internal error. when the http status code is 500, there may have detailled error message.
  + `400`  -bad request. when the http status code is 400, there will have detailled error message.
  + `401`  -unauthorized
  + `404`  -not found
