import dotenv from 'dotenv';
import path from 'path';
import fs from 'fs';

if(fs.existsSync(path.resolve(__dirname, '../.env'))) {
    dotenv.config({ path: path.resolve(__dirname, '../.env') });
}