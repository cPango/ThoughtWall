import * as threadActions from './thread.actions';

const initialState = {
    threadId: null,
};

export function ThreadReducer(state = initialState, action: threadActions.ChangeThread) {
    switch(action.type) {
        case threadActions.CHANGE_THREAD:
            return action.payload;
        default:
            return state;
    }
}