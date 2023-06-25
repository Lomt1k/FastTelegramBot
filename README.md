# FastTelegramBot
A simple telegram bot client to speed up json serialization and deserialization as much as possible (to handle a large number of simple requests)

Methods that use recursion are specifically avoided (JsonConvert.SerializeObject andJsonConvert.DeserializeObject)