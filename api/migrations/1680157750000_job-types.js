/* eslint-disable camelcase */

exports.shorthands = undefined;

exports.up = pgm => {
    pgm.createType('job_type', ['UserCredentials', 'AnyCredentials']);
};

exports.down = pgm => {
    pgm.dropType('job_type');
};
