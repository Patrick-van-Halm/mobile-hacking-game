const dotenv = require('dotenv');
const path = require('path');
const fs = require('fs');

if(fs.existsSync(path.resolve(__dirname, '.env'))) {
    dotenv.config({ path: path.resolve(__dirname, '.env') });
}